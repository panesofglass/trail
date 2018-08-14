namespace BlazorHosted1.Client

open Microsoft.AspNetCore.Blazor.Builder
open Microsoft.AspNetCore.Blazor.Hosting
open Microsoft.Extensions.DependencyInjection

type Startup() =

    member __.ConfigureServices(services:IServiceCollection) =
        ()

    member __.Configure(app:IBlazorApplicationBuilder) =
        app.AddComponent<App>("app")

    static member CreateHostBuilder() : IWebAssemblyHostBuilder =
        BlazorWebAssemblyHost
            .CreateDefaultBuilder()
            .UseBlazorStartup<Startup>()

(*
// TODO: can we go ALL the way?
module Program =
    open Microsoft.AspNetCore.Blazor.Hosting

    [<EntryPoint>]
    let main args =
        Startup.CreateHostBuilder().Build().Run()
        0
*)