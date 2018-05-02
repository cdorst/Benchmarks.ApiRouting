using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Default
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) => services.AddMvcCore();

        public void Configure(IApplicationBuilder app) => app.UseMvc();
    }
}
