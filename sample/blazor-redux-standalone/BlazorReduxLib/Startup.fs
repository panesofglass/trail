namespace BlazorApp1

open System
open Microsoft.AspNetCore.Blazor.Builder
open Microsoft.AspNetCore.Blazor.Hosting
open Microsoft.Extensions.DependencyInjection
open BlazorRedux
open Library1

type Startup() =

    member __.ConfigureServices(services:IServiceCollection) =
        services.AddReduxStore<MyModel, MyMsg>(
            initialState = { Location = ""; Count = 0; Forecasts = None },
            rootReducer = Reducer(Store.reducer),
            options = fun options ->
                options.LocationReducer <- Reducer(Store.locationReducer)
                options.GetLocation <- fun state -> state.Location
                options.StateSerializer <- Func<_,_>(Store.stateSerializer)
                options.StateDeserializer <- Func<_,_>(Store.stateDeserializer)
            )

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