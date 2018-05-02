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
