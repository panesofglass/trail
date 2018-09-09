using Microsoft.AspNetCore.Blazor.Hosting;
using BlazorApp1;

namespace BlazorApp1.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Startup.CreateHostBuilder().Build().Run();
        }
    }
}
