namespace BlazorApp1

open Microsoft.AspNetCore.Blazor.Routing
open Trail

type App() =
    inherit Trail.Component()

    override __.Render() =
        Dom.router<Router> typeof<App>.Assembly
