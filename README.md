# ASP.NET Core MVC middleware benchmarks

Run `./run.ps1` or `./run.sh` at the repository root to repeat the experiment

## Question

For GET-only APIs with simple types, what is the performance benefit (if any) of removing the MVC middleware and working with the routing middleware directly?

## Variables

Two applications are tested:

- Custom
- Default

The `Custom` application uses the `Routing` middleware (directly) to handle request-handling and response-writing.
`Default` uses the `MVC` layers of abstraction in addition to the `Routing` middleware.

## Hypothesis

Given that `MVC` is layered over the `Routing` middleware, removing those layers should result in less operations and a shorter average runtime for the tested scenario.

## Results

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.16299.371 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742186 Hz, Resolution=364.6726 ns, Timer=TSC
.NET Core SDK=2.1.300-preview2-008533
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-YRHFRF : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT

LaunchCount=10  

```
|         Method |     Mean |     Error |    StdDev | Rank |
|--------------- |---------:|----------:|----------:|-----:|
|  CustomRouting | 1.146 ms | 0.0094 ms | 0.0661 ms |    1 |
| DefaultRouting | 1.315 ms | 0.0090 ms | 0.0511 ms |    2 |

## Conclusion

As expected, requests with the `Routing` middleware directly (and not using `MVC`) result in faster average server response times.
