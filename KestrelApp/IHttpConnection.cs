using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace KestrelApp
{
    public interface IHttpConnection : IHttpHeadersHandler, IHttpRequestLineHandler
    {
        PipeReader Reader { get; set; }
        PipeWriter Writer { get; set; }
        Task ExecuteAsync();
        ValueTask OnReadCompletedAsync();
    }
}
