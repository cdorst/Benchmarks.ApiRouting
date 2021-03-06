﻿using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Entities.Constants;

namespace KestrelApp
{
    public partial class KestrelHttpApplication
    {
        private static readonly AsciiString _eoh = "\r\n\r\n"; // End Of Headers
        private static readonly AsciiString _http11OK = "HTTP/1.1 200 OK\r\n";
        private static readonly AsciiString _headerContentLength = "Content-Length: ";

        public void OnStartLine(HttpMethod method, HttpVersion version, Span<byte> target, Span<byte> path, Span<byte> query, Span<byte> customMethod, bool pathEncoded)
        {
        }

        public ValueTask ProcessRequestAsync()
        {
            var payload = EntityConstant.ToReadonlySpan();
            var writer = GetWriter(Writer);

            // HTTP 1.1 OK
            writer.Write(_http11OK);

            // Content-Length header
            writer.Write(_headerContentLength);
            writer.WriteNumeric((uint)payload.Length);

            // End of headers
            writer.Write(_eoh);

            // Body
            writer.Write(payload);
            writer.Commit();

            return default;
        }
    }

    public partial class KestrelHttpApplication : IHttpConnection
    {
        private State _state;

        public PipeReader Reader { get; set; }
        public PipeWriter Writer { get; set; }

        private HttpParser<ParsingAdapter> Parser { get; } = new HttpParser<ParsingAdapter>();

        public async Task ExecuteAsync()
        {
            try
            {
                await ProcessRequestsAsync();

                Reader.Complete();
            }
            catch (Exception ex)
            {
                Reader.Complete(ex);
            }
            finally
            {
                Writer.Complete();
            }
        }

        private async Task ProcessRequestsAsync()
        {
            while (true)
            {
                var task = Reader.ReadAsync();

                if (!task.IsCompleted)
                {
                    // No more data in the input
                    await OnReadCompletedAsync();
                }

                var result = await task;
                var buffer = result.Buffer;
                while (true)
                {
                    if (!ParseHttpRequest(ref buffer, result.IsCompleted, out var examined))
                    {
                        return;
                    }

                    if (_state == State.Body)
                    {
                        await ProcessRequestAsync();

                        _state = State.StartLine;

                        if (!buffer.IsEmpty)
                        {
                            // More input data to parse
                            continue;
                        }
                    }

                    // No more input or incomplete data, Advance the Reader
                    Reader.AdvanceTo(buffer.Start, examined);
                    break;
                }
            }
        }

        private bool ParseHttpRequest(ref ReadOnlySequence<byte> buffer, bool isCompleted, out SequencePosition examined)
        {
            examined = buffer.End;

            var consumed = buffer.Start;
            var state = _state;

            if (!buffer.IsEmpty)
            {
                if (state == State.StartLine)
                {
                    if (Parser.ParseRequestLine(new ParsingAdapter(this), buffer, out consumed, out examined))
                    {
                        state = State.Headers;
                    }

                    buffer = buffer.Slice(consumed);
                }

                if (state == State.Headers)
                {
                    if (Parser.ParseHeaders(new ParsingAdapter(this), buffer, out consumed, out examined, out int consumedBytes))
                    {
                        state = State.Body;
                    }

                    buffer = buffer.Slice(consumed);
                }

                if (state != State.Body && isCompleted)
                {
                    ThrowUnexpectedEndOfData();
                }
            }
            else if (isCompleted)
            {
                return false;
            }

            _state = state;
            return true;
        }

        public void OnHeader(Span<byte> name, Span<byte> value)
        {
        }

        public async ValueTask OnReadCompletedAsync()
        {
            await Writer.FlushAsync();
        }

        private static void ThrowUnexpectedEndOfData()
        {
            throw new InvalidOperationException("Unexpected end of data!");
        }

        private enum State : byte
        {
            StartLine,
            Headers,
            Body
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BufferWriter<WriterAdapter> GetWriter(PipeWriter pipeWriter)
            => new BufferWriter<WriterAdapter>(new WriterAdapter(pipeWriter));

        private struct WriterAdapter : IBufferWriter<byte>
        {
            public PipeWriter Writer;

            public WriterAdapter(PipeWriter writer)
                => Writer = writer;

            public void Advance(int count)
                => Writer.Advance(count);

            public Memory<byte> GetMemory(int sizeHint = 0)
                => Writer.GetMemory(sizeHint);

            public Span<byte> GetSpan(int sizeHint = 0)
                => Writer.GetSpan(sizeHint);
        }

        private struct ParsingAdapter : IHttpRequestLineHandler, IHttpHeadersHandler
        {
            public KestrelHttpApplication RequestHandler;

            public ParsingAdapter(KestrelHttpApplication requestHandler)
                => RequestHandler = requestHandler;

            public void OnHeader(Span<byte> name, Span<byte> value)
                => RequestHandler.OnHeader(name, value);

            public void OnStartLine(HttpMethod method, HttpVersion version, Span<byte> target, Span<byte> path, Span<byte> query, Span<byte> customMethod, bool pathEncoded)
                => RequestHandler.OnStartLine(method, version, target, path, query, customMethod, pathEncoded);
        }
    }
}
