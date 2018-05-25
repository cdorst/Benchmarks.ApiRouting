using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace KestrelApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder().Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder()
            => new WebHostBuilder()
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseKestrel((context, options)
                    => options.ListenAnyIP(44303, builder
                        => builder.UseHttpApplication<KestrelHttpApplication>()))
                .UseStartup<Startup>();
    }
}
