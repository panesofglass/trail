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
                Dom.input [
                    Attr.typ "number"
                    Attr.valuef "%i" this.CurrentCount
                    Attr.onchange(fun e -> this.CurrentCount <- int(unbox<string> e.Value))
                    Attr.isDisabled this.IsSelected
                ]
                Dom.input [
                    Attr.typ "checkbox"
                    Attr.isChecked this.IsSelected
                    Attr.onchange(fun e -> this.IsSelected <- unbox<bool> e.Value)
                ]
            ]
            Dom.button [ Attr.onclick(this.IncrementCount) ] [ Dom.text "Click me" ]
            Dom.button [ Attr.onclick(this.Select) ] [ Dom.text "Select" ]
        ]
    
    member val private CurrentCount : int = 0 with get, set
    member val private IsSelected : bool = false with get, set

    member this.IncrementCount _ =
        this.CurrentCount <- Library1.Counter.incrementBy(1, this.CurrentCount)
    
    member this.Select _ =
        this.IsSelected <- not this.IsSelected
