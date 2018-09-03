namespace BlazorApp1.Pages

open System.Net.Http
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open BlazorApp1.Shared
open Trail
open Library1

[<LayoutAttribute(typeof<MainLayout>)>]
[<Route("/fetchdata")>]
type FetchData () =
    inherit MyAppComponent()

    [<Inject>]
    member val private Http : HttpClient = Unchecked.defaultof<HttpClient> with get, set

    override this.OnInitAsync() =
        ActionCreators.loadWeather(BlazorRedux.Dispatcher this.Store.Dispatch, this.Http)

    override this.Render() =
        Dom.Fragment [
            yield Dom.h1 [] [Dom.text "Weather forecast"]
            yield Dom.p [] [Dom.text "This component domonstrates fetching data from the server."]
            match this.State.Forecasts with
            | None ->
                yield Dom.p [] [
                    Dom.em [] [Dom.text "Loading..."]
                ]
            | Some forecasts ->
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
