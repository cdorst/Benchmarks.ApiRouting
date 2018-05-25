using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using static Entities.Constants;

namespace Custom
{
    public class Startup
    {
        private const string id = nameof(id);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouter(routes =>
            {
                routes.MapGet("api/1/{id:int}", async context =>
                {
                    var payload = EntityConstant.ToReadonlyMemory();
                    var response = context.Response;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.ContentLength = payload.Length;
                    await response.Body.WriteAsync(payload);
                });
            });
        }
    }
}
