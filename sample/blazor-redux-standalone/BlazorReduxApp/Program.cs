using BlazorRedux;
using Library1;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(configure =>
            {
                // Add any custom services here
                configure.AddSingleton(
                    new Store<MyModel, MyMsg>(
                        Store.Reducer,
                        Store.LocationReducer,
                        (state) => state.Location,
                        new MyModel("", 0, null)));
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
