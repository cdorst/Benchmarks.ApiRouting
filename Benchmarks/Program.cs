using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Benchmarks.Constants;

namespace Benchmarks
{

    [SimpleJob(10)]
    [RPlotExporter, RankColumn]
    public class Tests
    {
        [Benchmark]
        public async Task<HttpResponseMessage> RoutingMiddleware()
            => await Custom.GetAsync(Route);

        [Benchmark]
        public async Task<HttpResponseMessage> MvcController()
            => await Default.GetAsync(Route);

        [Benchmark]
        public async Task<HttpResponseMessage> KestrelHttpApp()
            => await KestrelApp.GetAsync(Route);
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Tests>();
            WriteReadmeFile();
        }

        private static void WriteReadmeFile()
        {
            var readme = new StringBuilder()
                .Append("# ASP.NET Core MVC middleware benchmarks")
                .AppendLine()
                .AppendLine()
                .AppendLine("Run `./run.ps1` or `./run.sh` at the repository root to repeat the experiment")
                .AppendLine()
                .AppendLine("## Question")
                .AppendLine()
                .AppendLine("For GET-only APIs with simple types, what is the performance benefit (if any) of removing the MVC middleware and working with the routing middleware directly, or working with Kestrel directly?")
                .AppendLine()
                .AppendLine("## Variables")
                .AppendLine()
                .AppendLine("Three applications are tested:")
                .AppendLine()
                .AppendLine("- Custom (`RoutingMiddleware`)")
                .AppendLine("- Default (`MvcController`)")
                .AppendLine("- KestrelApp (`KestrelHttpApp`)")
                .AppendLine()
                .AppendLine("The `Custom` application uses the `Routing` middleware (directly) to handle request-handling and response-writing.")
                .AppendLine("`Default` uses the `MVC` layers of abstraction in addition to the `Routing` middleware.")
                .AppendLine("`KestrelApp` uses the Kestrel IConnectionBuilder extensions developed for the [Kestrel platform benchmarks](https://github.com/aspnet/KestrelHttpServer/tree/dev/benchmarkapps/PlatformBenchmarks).")
                .AppendLine()
                .AppendLine("## Hypothesis")
                .AppendLine()
                .AppendLine("Given that `MVC` is layered over the `Routing` middleware, removing those layers should result in less operations and a shorter average runtime for the tested scenario. Given that `Routing` middleware is layered over Kestrel, using the `IConnectionBuilder` \"Kestrel HTTP Application\" should result in even better performance than `Routing` or `MVC`.")
                .AppendLine()
                .AppendLine("## Results")
                .AppendLine();
            var dataTable = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "BenchmarkDotNet.Artifacts", "results", "Tests-report-github.md"));
            foreach (var line in dataTable)
                readme.AppendLine(line);
            readme.AppendLine()
                .AppendLine("## Conclusion")
                .AppendLine()
                .AppendLine("As expected, requests with the `Routing` middleware directly (and not using `MVC`) result in faster average server response times.")
                .AppendLine("As expected, requests with the \"Kestrel HTTP Application\" are even more performant than `Routing` or `MVC`.");
            File.WriteAllText("../README.md", readme.ToString());
        }
    }
}
