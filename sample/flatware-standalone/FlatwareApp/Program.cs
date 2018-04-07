using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using System;
using Flatware;
using Library1;

namespace BlazorApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(configure =>
            {
                // Add any custom services here
                configure.AddFlatware<MyMsg, MyMdl>(MyMdl.Init);
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
