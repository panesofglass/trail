namespace Library1

module Say =
    [<CompiledName("Hello")>]
    let hello name =
        sprintf "Hello %s" name

module Counter =
    [<CompiledName("IncrementBy")>]
    let incrementBy (value, input) = input + value

module FetchData =
    open System
    open System.Net.Http
    open Microsoft.AspNetCore.Blazor

    type WeatherForecast () =
        member val Date = Unchecked.defaultof<DateTime> with get, set
        member val TemperatureC = Unchecked.defaultof<int> with get, set
        member val TemperatureF = Unchecked.defaultof<int> with get, set
        member val Summary = Unchecked.defaultof<string> with get, set

    [<CompiledName("FetchForecastsAsync")>]
    let fetchForecasts (http:HttpClient) =
        async {
            return! Async.AwaitTask <| http.GetJsonAsync<WeatherForecast[]>("/sample-data/weather.json")
        }
        |> Async.StartAsTask

(*
namespace BlazorApp1

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open Microsoft.AspNetCore.Blazor.Routing

// NOTE: _ViewImports.cshtml appear to be required in the BlazorApp.
type _ViewImports () =
    inherit BlazorComponent()
    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
*)

namespace BlazorApp1.Shared

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open Microsoft.AspNetCore.Blazor.Routing

type NavMenu () =
    inherit BlazorComponent()

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        builder.OpenElement(0, "div")
        builder.AddAttribute(1, "class", "main-nav")
        builder.AddContent(2, "\n    ")
        builder.OpenElement(3, "div")
        builder.AddAttribute(4, "class", "navbar navbar-inverse")
        builder.AddContent(5, "\n        ")
        builder.OpenElement(6, "div")
        builder.AddAttribute(7, "class", "navbar-header")
        builder.AddContent(8, "\n            ")
        builder.OpenElement(9, "button")
        builder.AddAttribute(10, "type", "button")
        builder.AddAttribute(11, "class", "navbar-toggle")
        builder.AddAttribute(12, "data-toggle", "collapse")
        builder.AddAttribute(13, "data-target", ".navbar-collapse")
        builder.AddContent(14, "\n                ")
        builder.OpenElement(15, "span")
        builder.AddAttribute(16, "class", "sr-only")
        builder.AddContent(17, "Toggle navigation")
        builder.CloseElement()
        builder.AddContent(18, "\n                ")
        builder.OpenElement(19, "span")
        builder.AddAttribute(20, "class", "icon-bar")
        builder.CloseElement()
        builder.AddContent(21, "\n                ")
        builder.OpenElement(22, "span")
        builder.AddAttribute(23, "class", "icon-bar")
        builder.CloseElement()
        builder.AddContent(24, "\n                ")
        builder.OpenElement(25, "span")
        builder.AddAttribute(26, "class", "icon-bar")
        builder.CloseElement()
        builder.AddContent(27, "\n            ")
        builder.CloseElement()
        builder.AddContent(28, "\n            ")
        builder.OpenElement(29, "a")
        builder.AddAttribute(30, "class", "navbar-brand")
        builder.AddAttribute(31, "href", "/")
        builder.AddContent(32, "BlazorApp1")
        builder.CloseElement()
        builder.AddContent(33, "\n        ")
        builder.CloseElement()
        builder.AddContent(34, "\n        ")
        builder.OpenElement(35, "div")
        builder.AddAttribute(36, "class", "clearfix")
        builder.CloseElement()
        builder.AddContent(37, "\n        ")
        builder.OpenElement(38, "div")
        builder.AddAttribute(39, "class", "navbar-collapse collapse")
        builder.AddContent(40, "\n            ")
        builder.OpenElement(41, "ul")
        builder.AddAttribute(42, "class", "nav navbar-nav")
        builder.AddContent(43, "\n                ")
        builder.OpenElement(44, "li")
        builder.AddContent(45, "\n                    ")
        builder.OpenComponent<Microsoft.AspNetCore.Blazor.Routing.NavLink>(46)
        builder.AddAttribute(47, "href", "/")
        builder.AddAttribute(48, "Match", NavLinkMatch.All)
        builder.AddAttribute(49, "ChildContent", (Microsoft.AspNetCore.Blazor.RenderFragment(fun builder2 ->
            builder2.AddContent(50, "\n                        ")
            builder2.OpenElement(51, "span")
            builder2.AddAttribute(52, "class", "glyphicon glyphicon-home")
            builder2.CloseElement()
            builder2.AddContent(53, " Home\n                    ")
        )))
        builder.CloseComponent()
        builder.AddContent(54, "\n                ")
        builder.CloseElement()
        builder.AddContent(55, "\n                ")
        builder.OpenElement(56, "li")
        builder.AddContent(57, "\n                    ")
        builder.OpenComponent<Microsoft.AspNetCore.Blazor.Routing.NavLink>(58)
        builder.AddAttribute(59, "href", "/counter")
        builder.AddAttribute(60, "ChildContent", (Microsoft.AspNetCore.Blazor.RenderFragment(fun builder2 ->
            builder2.AddContent(61, "\n                        ")
            builder2.OpenElement(62, "span")
            builder2.AddAttribute(63, "class", "glyphicon glyphicon-education")
            builder2.CloseElement()
            builder2.AddContent(64, " Counter\n                    ")
        )))
        builder.CloseComponent()
        builder.AddContent(65, "\n                ")
        builder.CloseElement()
        builder.AddContent(66, "\n                ")
        builder.OpenElement(67, "li")
        builder.AddContent(68, "\n                    ")
        builder.OpenComponent<Microsoft.AspNetCore.Blazor.Routing.NavLink>(69)
        builder.AddAttribute(70, "href", "/fetchdata")
        builder.AddAttribute(71, "ChildContent", (Microsoft.AspNetCore.Blazor.RenderFragment(fun builder2 ->
            builder2.AddContent(72, "\n                        ")
            builder2.OpenElement(73, "span")
            builder2.AddAttribute(74, "class", "glyphicon glyphicon-th-list")
            builder2.CloseElement()
            builder2.AddContent(75, " Fetch data\n                    ")
        )))
        builder.CloseComponent()
        builder.AddContent(76, "\n                ")
        builder.CloseElement()
        builder.AddContent(77, "\n            ")
        builder.CloseElement()
        builder.AddContent(78, "\n        ")
        builder.CloseElement()
        builder.AddContent(79, "\n    ")
        builder.CloseElement()
        builder.AddContent(80, "\n")
        builder.CloseElement()
        builder.AddContent(81, "\n")


type MainLayout () =
    inherit BlazorComponent()

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        builder.AddContent(0, "\n")
        builder.OpenElement(1, "div")
        builder.AddAttribute(2, "class", "container-fluid")
        builder.AddContent(3, "\n    ")
        builder.OpenElement(4, "div")
        builder.AddAttribute(5, "class", "row")
        builder.AddContent(6, "\n        ")
        builder.OpenElement(7, "div")
        builder.AddAttribute(8, "class", "col-sm-3")
        builder.AddContent(9, "\n            ")
        builder.OpenComponent<NavMenu>(10)
        builder.CloseComponent()
        builder.AddContent(11, "\n        ")
        builder.CloseElement()
        builder.AddContent(12, "\n        ")
        builder.OpenElement(13, "div")
        builder.AddAttribute(14, "class", "col-sm-9")
        builder.AddContent(15, "\n            ")
        builder.AddContent(16, this.Body)
        builder.AddContent(17, "\n        ")
        builder.CloseElement()
        builder.AddContent(18, "\n    ")
        builder.CloseElement()
        builder.AddContent(19, "\n")
        builder.CloseElement()
        builder.AddContent(20, "\n\n")

    member val Body : RenderFragment = Unchecked.defaultof<RenderFragment> with get, set

    interface ILayoutComponent with
        member this.Body with get() = this.Body
                          and set(value) = this.Body <- value

type SurveyPrompt () =
    inherit BlazorComponent()

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        builder.OpenElement(0, "div")
        builder.AddAttribute(1, "class", "alert alert-survey")
        builder.AddAttribute(2, "role", "alert")
        builder.AddContent(3, "\n    ")
        builder.OpenElement(4, "span")
        builder.AddAttribute(5, "class", "glyphicon glyphicon-ok-circle")
        builder.AddAttribute(6, "aria-hidden", "true")
        builder.CloseElement()
        builder.AddContent(7, "\n    ")
        builder.OpenElement(8, "strong")
        builder.AddContent(9, this.Title)
        builder.CloseElement()
        builder.AddContent(10, "\n\n    Please take our\n    ")
        builder.OpenElement(11, "a")
        builder.AddAttribute(12, "target", "_blank")
        builder.AddAttribute(13, "class", "alert-link")
        builder.AddAttribute(14, "href", "https://go.microsoft.com/fwlink/?linkid=870381")
        builder.AddContent(15, "\n        brief survey\n    ")
        builder.CloseElement()
        builder.AddContent(16, "\n    and tell us what you think.\n")
        builder.CloseElement()
        builder.AddContent(17, "\n\n")

    // This is to demonstrate how a parent component can supply parameters
    member val Title : string = Unchecked.defaultof<string> with get, set

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
open FSharp.Blazor

(*
// NOTE: _ViewImports.cshtml appear to be required in the BlazorApp.
[<LayoutAttribute(typeof<MainLayout>)>]
type _ViewImports () =
    inherit BlazorComponent()
    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
*)

[<LayoutAttribute(typeof<MainLayout>)>]
[<RouteAttribute("/")>]
type Index () =
    inherit BlazorComponent()
    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        (*
        builder.AddContent(0, "\n")
        builder.OpenElement(1, "h1")
        builder.AddContent(2, "Hello, world!")
        builder.CloseElement()
        builder.AddContent(3, "\n\nWelcome to your new app.\n\n")
        builder.OpenComponent<SurveyPrompt>(4)
        builder.AddAttribute(5, "title", "How is Blazor working for you?")
        builder.CloseComponent()
        builder.AddContent(6, "\n")
        *)
        Dom.Document [
            Dom.el "h1" [] [ Dom.text "Hello, world!" ]
            Dom.text "\n\nWelcome to your new app.\n\n"
            Dom.comp<SurveyPrompt> [ "title", "How is Blazor working for you?" ]
        ]
        |> RenderTree.build builder

(*
[<LayoutAttribute(typeof<MainLayout>)>]
[<RouteAttribute("/counter")>]
type Counter () =
    inherit BlazorComponent()
    
    let mutable currentCount = 0

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        builder.OpenElement(0, "h1")
        builder.AddContent(1, "Counter")
        builder.CloseElement()
        builder.AddContent(2, "\n\n")
        builder.OpenElement(3, "p")
        builder.AddContent(4, "Current count: ")
        builder.AddContent(5, currentCount)
        builder.CloseElement()
        builder.AddContent(6, "\n\n")
        builder.OpenElement(7, "button")
        // TODO: determine how to create the onclick RenderTree.RenderTreeFrame
        builder.AddAttribute(8, onclick(IncrementCount))
        // NOTE: this may be an option ...
        //builder.AddAttribute(8, "onclick", UIEventHandler.CreateDelegate(IncrementCount))
        builder.AddContent(9, "Click me")
        builder.CloseElement()
        builder.AddContent(10, "\n\n")

    member this.IncrementCount () =
        currentCount = Library1.Counter.incrementBy(1, currentCount)
*)

[<LayoutAttribute(typeof<MainLayout>)>]
[<Route("/fetchdata")>]
type FetchData () =
    inherit BlazorComponent()
    
    let mutable forecasts : Library1.FetchData.WeatherForecast[] = [||]

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        builder.AddContent(0, "\n")
        builder.OpenElement(1, "h1")
        builder.AddContent(2, "Weather forecast")
        builder.CloseElement()
        builder.AddContent(3, "\n\n")
        builder.OpenElement(4, "p")
        builder.AddContent(5, "This component demonstrates fetching data from the server.")
        builder.CloseElement()
        builder.AddContent(6, "\n\n")
        if Array.isEmpty forecasts then
            builder.AddContent(7, "    ")
            builder.OpenElement(8, "p")
            builder.OpenElement(9, "em")
            builder.AddContent(10, "Loading...")
            builder.CloseElement()
            builder.CloseElement()
            builder.AddContent(11, "\n")
        else
            builder.AddContent(12, "    ")
            builder.OpenElement(13, "table")
            builder.AddAttribute(14, "class", "table")
            builder.AddContent(15, "\n        ")
            builder.OpenElement(16, "thead")
            builder.AddContent(17, "\n            ")
            builder.OpenElement(18, "tr")
            builder.AddContent(19, "\n                ")
            builder.OpenElement(20, "th")
            builder.AddContent(21, "Date")
            builder.CloseElement()
            builder.AddContent(22, "\n                ")
            builder.OpenElement(23, "th")
            builder.AddContent(24, "Temp. (C)")
            builder.CloseElement()
            builder.AddContent(25, "\n                ")
            builder.OpenElement(26, "th")
            builder.AddContent(27, "Temp. (F)")
            builder.CloseElement()
            builder.AddContent(28, "\n                ")
            builder.OpenElement(29, "th")
            builder.AddContent(30, "Summary")
            builder.CloseElement()
            builder.AddContent(31, "\n            ")
            builder.CloseElement()
            builder.AddContent(32, "\n        ")
            builder.CloseElement()
            builder.AddContent(33, "\n        ")
            builder.OpenElement(34, "tbody")
            builder.AddContent(35, "\n")
            for forecast in forecasts do
                builder.AddContent(36, "                ")
                builder.OpenElement(37, "tr")
                builder.AddContent(38, "\n                    ")
                builder.OpenElement(39, "td")
                builder.AddContent(40, forecast.Date.ToShortDateString())
                builder.CloseElement()
                builder.AddContent(41, "\n                    ")
                builder.OpenElement(42, "td")
                builder.AddContent(43, forecast.TemperatureC)
                builder.CloseElement()
                builder.AddContent(44, "\n                    ")
                builder.OpenElement(45, "td")
                builder.AddContent(46, forecast.TemperatureF)
                builder.CloseElement()
                builder.AddContent(47, "\n                    ")
                builder.OpenElement(48, "td")
                builder.AddContent(49, forecast.Summary)
                builder.CloseElement()
                builder.AddContent(50, "\n                ")
                builder.CloseElement()
                builder.AddContent(51, "\n")
            builder.AddContent(52, "        ")
            builder.CloseElement()
            builder.AddContent(53, "\n    ")
            builder.CloseElement()
            builder.AddContent(54, "\n")
        builder.AddContent(55, "\n")

    override this.OnInitAsync() =
        async {
            let! result = Async.AwaitTask <| Library1.FetchData.fetchForecasts(this.Http)
            forecasts <- result
        }
        |> Async.StartAsTask :> System.Threading.Tasks.Task

    [<Inject>]
    member val private Http : HttpClient = Unchecked.defaultof<HttpClient> with get, set

namespace BlazorApp1

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Browser.Rendering
open Microsoft.AspNetCore.Blazor.Browser.Services
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open Microsoft.AspNetCore.Blazor.Routing
open BlazorApp1
open BlazorApp1.Shared

type Marker = class end

type App() =
    inherit BlazorComponent()

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        builder.AddContent(0, "\n")
        builder.OpenComponent<Router>(1)
        builder.AddAttribute(2, "AppAssembly", box typeof<Marker>.Assembly)
        builder.CloseComponent()
        builder.AddContent(3, "\n")

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