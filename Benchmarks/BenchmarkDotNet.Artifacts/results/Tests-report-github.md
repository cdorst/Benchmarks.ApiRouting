``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.300-rc1-008673
  [Host] : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT
  Core   : .NET Core 2.1.0-rc1 (CoreCLR 4.6.26426.02, CoreFX 4.6.26426.04), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|         Method |       Mean |     Error |    StdDev | Rank |
|--------------- |-----------:|----------:|----------:|-----:|
|  CustomRouting |   713.0 us |  14.26 us |  25.71 us |    1 |
| DefaultRouting |   824.4 us |  16.04 us |  24.50 us |    2 |
| KestrelHttpApp | 3,423.6 us | 109.85 us | 320.43 us |    3 |
