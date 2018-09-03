namespace BlazorApp1

open Microsoft.AspNetCore.Blazor.Routing
open Trail
open Library1

type App() =
    inherit MyAppComponent()

    override __.Render() =
        Dom.Fragment [
            Dom.router<Router> typeof<App>.Assembly
            Dom.comp<BlazorRedux.ReduxDevTools> [] []
        ]


(*
// TODO: can we go ALL the way?
module Program =
    [<EntryPoint>]
    let main _ =
        let serviceProvider = new BrowserServiceProvider(fun configure ->
            // Add any custom services here
            ()
        )

        (new BrowserRenderer(serviceProvider)).AddComponent<App>("app")
        0
*)
