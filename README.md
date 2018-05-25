# ASP.NET Core MVC middleware benchmarks

Run `./run.ps1` or `./run.sh` at the repository root to repeat the experiment

## Question

For GET-only APIs with simple types, what is the performance benefit (if any) of removing the MVC middleware and working with the routing middleware directly, or working with Kestrel directly?

## Variables

Three applications are tested:

- Custom (`RoutingMiddleware`)
- Default (`MvcController`)
- KestrelApp (`KestrelHttpApp`)

The `Custom` application uses the `Routing` middleware (directly) to handle request-handling and response-writing.
`Default` uses the `MVC` layers of abstraction in addition to the `Routing` middleware.
`KestrelApp` uses the Kestrel IConnectionBuilder extensions developed for the [Kestrel platform benchmarks](https://github.com/aspnet/KestrelHttpServer/tree/dev/benchmarkapps/PlatformBenchmarks).

## Hypothesis

Given that `MVC` is layered over the `Routing` middleware, removing those layers should result in less operations and a shorter average runtime for the tested scenario. Given that `Routing` middleware is layered over Kestrel, using the `IConnectionBuilder` "Kestrel HTTP Application" should result in even better performance than `Routing` or `MVC`.

## Results

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.300-rc1-008673
  [Host]     : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT
  Job-UECWRA : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT

LaunchCount=10  

```
|            Method |     Mean |    Error |   StdDev | Rank |
|------------------ |---------:|---------:|---------:|-----:|
| RoutingMiddleware | 685.6 us | 4.705 us | 23.93 us |    2 |
|     MvcController | 789.8 us | 5.863 us | 26.13 us |    3 |
|    KestrelHttpApp | 632.3 us | 4.611 us | 23.70 us |    1 |

## Conclusion

As expected, requests with the `Routing` middleware directly (and not using `MVC`) result in faster average server response times.
As expected, requests with the "Kestrel HTTP Application" are even more performant than `Routing` or `MVC`.
