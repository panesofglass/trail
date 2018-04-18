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

[<LayoutAttribute(typeof<MainLayout>)>]
[<RouteAttribute("/counter")>]
type Counter () =
    inherit Trail.Component()

    override this.Render() =
        Dom.Fragment [
            Dom.h1 [] [Dom.text "Counter"]
            Dom.p [] [
                Dom.text "Current count: "
                Dom.textf "%i" this.CurrentCount
            ]
            Dom.button [
                    Attr.onclick(this.IncrementCount)
                ] [
                    Dom.text "Click me"
                ]
        ]
    
    member val private CurrentCount : int = 0 with get, set

    member this.IncrementCount _ =
        this.CurrentCount <- Library1.Counter.incrementBy(1, this.CurrentCount)
