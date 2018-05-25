using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Default.Controllers
{
    internal class ReadOnlyMemoryActionResult : IActionResult
    {
        private readonly ReadOnlyMemory<byte> _payload;

        public ReadOnlyMemoryActionResult(in ReadOnlyMemory<byte> bytes)
        {
            _payload = bytes;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentLength = _payload.Length;
            return response.Body.WriteAsync(_payload).AsTask();
        }
    }
}
