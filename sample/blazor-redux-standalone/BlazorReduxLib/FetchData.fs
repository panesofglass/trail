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

[<LayoutAttribute(typeof<MainLayout>)>]
[<Route("/fetchdata")>]
type FetchData () =
    inherit MyAppComponent()

    override this.Render() =
        Dom.Fragment [
            yield Dom.h1 [] [Dom.text "Weather forecast"]
            yield Dom.p [] [Dom.text "This component domonstrates fetching data from the server."]
            match this.State.Forecasts with
            | None | Some [||] ->
                yield Dom.p [] [
                    Dom.em [] [Dom.text "Loading..."]
                ]
            | Some forecasts ->
                yield Dom.table [Dom.HtmlAttribute("class", "table")] [
                    Dom.thead [] [
                        Dom.tr [] [
                            Dom.th [] [Dom.text "Date"]
                            Dom.th [] [Dom.text "Temp. (C)"]
                            Dom.th [] [Dom.text "Temp. (F)"]
                            Dom.th [] [Dom.text "Summary"]
                        ]
                    ]
                    Dom.tbody [] [
                        for forecast in forecasts ->
                            Dom.tr [] [
                                Dom.td [] [Dom.text (forecast.Date.ToShortDateString())]
                                Dom.td [] [Dom.textf "%i" forecast.TemperatureC]
                                Dom.td [] [Dom.textf "%i" forecast.TemperatureF]
                                Dom.td [] [Dom.text forecast.Summary]
                            ]
                    ]
                ]
        ]

    override this.OnInitAsync() =
        this.DispatchAsync(ActionCreators.LoadWeather(this.Http))
    
    [<Inject>]
    member val private Http : HttpClient = Unchecked.defaultof<HttpClient> with get, set