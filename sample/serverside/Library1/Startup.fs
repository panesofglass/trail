namespace BlazorApp1

open Microsoft.AspNetCore.Blazor.Builder
open Microsoft.AspNetCore.Blazor.Hosting
open Microsoft.Extensions.DependencyInjection
open Library1.FetchData

type Startup() =

    member __.ConfigureServices(services:IServiceCollection) =
        services.AddSingleton(forecastService) |> ignore
        // Or use a class and the generic overload that will construct the instance.
        // services.AddSingleton<WeatherForecastService>() |> ignore

    member __.Configure(app:IBlazorApplicationBuilder) =
        app.AddComponent<App>("app")

    static member CreateHostBuilder() : IWebAssemblyHostBuilder =
        BlazorWebAssemblyHost
            .CreateDefaultBuilder()
            .UseBlazorStartup<Startup>()
