namespace BlazorApp1.Pages

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open Microsoft.AspNetCore.Blazor.Routing
open BlazorApp1
open BlazorApp1.Shared
open Trail
open Library1

[<Layout(typeof<MainLayout>)>]
[<Route("/counter")>]
type Counter () =
    inherit MyAppComponent()

    override this.Render() =
        Dom.Fragment [
            Dom.h1 [] [Dom.text "Counter"]
            Dom.p [] [
                Dom.text "Current count: "
                Dom.textf "%i" this.State.Count
            ]
            Dom.button [
                    Attr.onclick(fun _ -> this.Dispatch(MyMsg.IncrementByOne))
                ] [
                    Dom.text "Click me"
                ]
        ]
    