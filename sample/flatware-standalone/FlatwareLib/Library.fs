namespace Library1

open System
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open FSharp.Control.Tasks

type MyMsg =
    | Increment of n : int
    | LoadWeather

type WeatherForecast() =
    member val Date = DateTime.MinValue with get, set
    member val TemperatureC = 0 with get, set
    member val TemperatureF = 0 with get, set
    member val Summary = "" with get, set

type MyMdl = { Count : int; Forecasts : WeatherForecast list } with
    static member Init = { Count = 0; Forecasts = [] }

[<AbstractClass>]
type MyAppComponent() =
    inherit Trail.FlatwareComponent<MyMsg, MyMdl>()

    [<Inject>]
    member val Http = null : HttpClient with get, set

    override this.ReduceAsync(msg : MyMsg, mdl : MyMdl) =
        task {
            match msg with
                | Increment n -> 
                    return { mdl with Count = mdl.Count + n }
                | LoadWeather -> 
                    let! forecasts = this.Http.GetJsonAsync<WeatherForecast[]>("/sample-data/weather.json") |> Async.AwaitTask
                    return { mdl with Forecasts = Array.toList forecasts }
        }
