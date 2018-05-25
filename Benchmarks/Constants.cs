using System;
using System.Net.Http;

namespace Benchmarks
{
    internal static class Constants
    {
        public const string Route = "/api/1/5";

        public static readonly HttpClient Custom = new HttpClient() { BaseAddress = new Uri("https://localhost:44355") };
        public static readonly HttpClient Default = new HttpClient() { BaseAddress = new Uri("https://localhost:44387") };
        public static readonly HttpClient KestrelApp = new HttpClient() { BaseAddress = new Uri("https://localhost:44303") };
    }
}
