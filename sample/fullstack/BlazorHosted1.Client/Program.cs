using Microsoft.AspNetCore.Blazor.Hosting;

namespace BlazorHosted1.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Startup.CreateHostBuilder().Build().Run();
        }
    }
}
