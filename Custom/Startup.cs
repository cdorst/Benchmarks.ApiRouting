using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

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
                routes.MapGet("api/1/{id:int}", context =>
                {
                    return context.Response.WriteAsync($"Id: {context.GetRouteValue(id)}");
                });
            });
        }
    }
}
