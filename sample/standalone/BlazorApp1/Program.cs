using Microsoft.AspNetCore.Blazor.Hosting;
using System;

namespace BlazorApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Startup.CreateHostBuilder().Build().Run();
        }
    }
}
