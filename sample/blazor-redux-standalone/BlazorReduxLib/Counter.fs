namespace BlazorApp1.Pages

open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open BlazorApp1.Shared
open Trail
open Library1

[<LayoutAttribute(typeof<MainLayout>)>]
[<RouteAttribute("/counter")>]
type Counter () =
    inherit MyAppComponent()

    override this.Render() =
        Dom.Fragment [
            Dom.h1 [] [Dom.text "Counter"]
            Dom.p [] [
                Dom.text "Current count: "
                Dom.input [
                    Attr.typ "number"
                    Attr.valuef "%i" this.State.Count
                    Attr.onchange(fun e ->
                        let value = int(unbox<string> e.Value)
                        this.Dispatch(MyMsg.UpdateValue value)
                    )
                ]
            ]
            Dom.button [
                Attr.className "btn btn-primary"
                Attr.onclick(fun _ -> this.Dispatch(MyMsg.IncrementByOne))
            ] [
                Dom.text "Click me"
            ]
        ]
    