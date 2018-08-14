namespace BlazorHosted1.Client.Pages

open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open BlazorHosted1.Client
open BlazorHosted1.Client.Shared
open Trail

[<LayoutAttribute(typeof<MainLayout>)>]
[<RouteAttribute("/counter")>]
type Counter () =
    inherit Trail.Component()

    let mutable currentCount = 0

    let mutable isSelected = false

    let incrementCount _ =
        currentCount <- Counter.incrementBy(1, currentCount)
    
    let select _ =
        isSelected <- not isSelected

    override this.Render() =
        Dom.Fragment [
            Dom.h1 [] [Dom.text "Counter"]
            Dom.p [] [
                Dom.text "Current count: "
                Dom.input [
                    Attr.typ "number"
                    Attr.valuef "%i" currentCount
                    Attr.onchange(fun e -> currentCount <- int(unbox<string> e.Value))
                    Attr.isDisabled isSelected
                ]
                Dom.input [
                    Attr.typ "checkbox"
                    Attr.isChecked isSelected
                    Attr.onchange(fun e -> isSelected <- unbox<bool> e.Value)
                ]
            ]
            Dom.button [Attr.className "btn btn-primary"; Attr.onclick incrementCount] [Dom.text "Click me"]
            Dom.button [Attr.className "btn btn-primary"; Attr.onclick select] [Dom.text "Select"]
        ]
    