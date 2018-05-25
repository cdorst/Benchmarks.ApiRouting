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
        public async Task<HttpResponseMessage> CustomRouting()
            => await Custom.GetAsync(Route);

        [Benchmark]
        public async Task<HttpResponseMessage> DefaultRouting()
            => await Default.GetAsync(Route);

        [Benchmark]
        public async Task<HttpResponseMessage> KestrelHttpApp()
            => await KestrelApp.GetAsync(Route);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Tests>();
            var readme = new StringBuilder()
                .Append("# ASP.NET Core MVC middleware benchmarks")
                .AppendLine()
                .AppendLine()
                .AppendLine("Run `./run.ps1` or `./run.sh` at the repository root to repeat the experiment")
                .AppendLine()
                .AppendLine("## Question")
                .AppendLine()
                .AppendLine("For GET-only APIs with simple types, what is the performance benefit (if any) of removing the MVC middleware and working with the routing middleware directly?")
                .AppendLine()
                .AppendLine("## Variables")
                .AppendLine()
                .AppendLine("Two applications are tested:")
                .AppendLine()
                .AppendLine("- Custom")
                .AppendLine("- Default")
                .AppendLine()
                .AppendLine("The `Custom` application uses the `Routing` middleware (directly) to handle request-handling and response-writing.")
                .AppendLine("`Default` uses the `MVC` layers of abstraction in addition to the `Routing` middleware.")
                .AppendLine()
                .AppendLine("## Hypothesis")
                .AppendLine()
                .AppendLine("Given that `MVC` is layered over the `Routing` middleware, removing those layers should result in less operations and a shorter average runtime for the tested scenario.")
                .AppendLine()
                .AppendLine("## Results")
                .AppendLine();
            var dataTable = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "BenchmarkDotNet.Artifacts", "results", "Tests-report-github.md"));
            foreach (var line in dataTable)
                readme.AppendLine(line);
            readme.AppendLine()
                .AppendLine("## Conclusion")
                .AppendLine()
                .AppendLine("As expected, requests with the `Routing` middleware directly (and not using `MVC`) result in faster average server response times.");
            File.WriteAllText("../README.md", readme.ToString());
        }
    }
}
