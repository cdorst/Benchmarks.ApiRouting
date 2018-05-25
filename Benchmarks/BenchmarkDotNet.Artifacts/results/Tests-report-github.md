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
