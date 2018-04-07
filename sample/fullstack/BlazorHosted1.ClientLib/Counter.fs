namespace BlazorHosted1.Client.Pages

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open Microsoft.AspNetCore.Blazor.Routing
open BlazorHosted1.Client
open BlazorHosted1.Client.Shared
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
                    Dom.BlazorFrameAttribute(this.onclick(Action this.IncrementCount))
                ] [
                    Dom.text "Click me"
                ]
        ]
    
    member val private CurrentCount : int = 0 with get, set

    member this.IncrementCount () =
        this.CurrentCount <- Counter.incrementBy(1, this.CurrentCount)
