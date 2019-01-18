namespace BlazorApp1.Pages

open System
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open BlazorApp1.Shared
open Library1.FetchData
open Trail

[<LayoutAttribute(typeof<MainLayout>)>]
[<Route("/fetchdata")>]
type FetchData () =
    inherit Trail.Component()

    let mutable forecasts : WeatherForecast[] = [||]

    [<Inject>]
    member val private ForecastService : IForecastService = Unchecked.defaultof<IForecastService> with get, set

    override this.OnInitAsync() =
        async {
            let! result = Async.AwaitTask <| this.ForecastService.FetchForecastsAsync(DateTime.Now)
            forecasts <- result
        }
        |> Async.StartAsTask :> System.Threading.Tasks.Task

    override this.Render() =
        Dom.Fragment [
            yield Dom.h1 [] [Dom.text "Weather forecast"]
            yield Dom.p [] [Dom.text "This component domonstrates fetching data from the server."]
            if Array.isEmpty forecasts then
                yield Dom.p [] [
                    Dom.em [] [Dom.text "Loading..."]
                ]
            else
                yield Dom.table [Attr.className "table"] [
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
