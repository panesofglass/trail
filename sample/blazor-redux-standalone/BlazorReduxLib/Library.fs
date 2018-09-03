namespace Library1

open System
open BlazorRedux
open Chiron

type WeatherForecast () =
    member val Date = Unchecked.defaultof<DateTime> with get, set
    member val TemperatureC = Unchecked.defaultof<int> with get, set
    member val TemperatureF = Unchecked.defaultof<int> with get, set
    member val Summary = Unchecked.defaultof<string> with get, set

    static member FromJson (_:WeatherForecast) =
        json {
            let! d = Json.read "Date"
            let! c = Json.read "TemperatureC"
            let! f = Json.read "TemperatureF"
            let! s = Json.read "Summary"
            return WeatherForecast(Date = d, TemperatureC = c, TemperatureF = f, Summary = s)
        }

    static member ToJson (x:WeatherForecast) =
        json {
            do! Json.write "Date" x.Date
            do! Json.write "TemperatureC" x.TemperatureC
            do! Json.write "TemperatureF" x.TemperatureF
            do! Json.write "Summary" x.Summary
        }

(*
type WeatherForecast =
    {
        Date : DateTime
        TemperatureC : int
        TemperatureF : int
        Summary : string
    }

    static member FromJson (_:WeatherForecast) =
        json {
            let! d = Json.read "Date"
            let! c = Json.read "TemperatureC"
            let! f = Json.read "TemperatureF"
            let! s = Json.read "Summary"
            return { Date = d; TemperatureC = c; TemperatureF = f; Summary = s }
        }

    static member ToJson (x:WeatherForecast) =
        json {
            do! Json.write "Date" x.Date
            do! Json.write "TemperatureC" x.TemperatureC
            do! Json.write "TemperatureF" x.TemperatureF
            do! Json.write "Summary" x.Summary
        }
*)

type MyModel =
    {
        Location : string
        Count : int
        Forecasts : WeatherForecast [] option
    }

    static member FromJson(_:MyModel) =
        json {
            let! l = Json.read "Location"
            let! c = Json.read "Count"
            let! f = Json.read "Forecasts"
            return { Location = l; Count = c; Forecasts = f }
        }

    static member ToJson(x:MyModel) =
        json {
            do! Json.write "Location" x.Location
            do! Json.write "Count" x.Count
            do! Json.write "Forecasts" x.Forecasts
        }

type MyMsg =
    | IncrementByOne
    | IncrementByValue of n : int
    | UpdateValue of n : int
    | ClearWeather
    | ReceiveWeather of r : WeatherForecast[]

module ActionCreators =
    open System.Net.Http
    open System.Threading.Tasks
    open Microsoft.JSInterop
    open FSharp.Control.Tasks

    [<CompiledName("LoadWeather")>]
    let loadWeather (dispatch: Dispatcher<MyMsg>, http: HttpClient) =
        task {
            dispatch.Invoke(MyMsg.ClearWeather) |> ignore
            let! forecastString = http.GetStringAsync("/sample-data/weather.json")
            //let json = Json.parse forecastString
            //let forecasts : WeatherForecast list = Json.deserialize json
            let forecasts : WeatherForecast[] = Json.Deserialize(forecastString)
            dispatch.Invoke(MyMsg.ReceiveWeather forecasts) |> ignore
        } :> Task

module Store =

    [<CompiledName("Reducer")>]
    let reducer state action =
        match action with
        | IncrementByOne -> { state with Count = state.Count + 1 }
        | IncrementByValue n -> { state with Count = state.Count + n }
        | UpdateValue n -> { state with Count = n }
        | ClearWeather -> { state with Forecasts = None }
        | ReceiveWeather f -> { state with Forecasts = Some f }
    
    [<CompiledName("LocationReducer")>]
    let locationReducer state (action:NewLocationAction) =
        { state with Location = action.Location }
    
    [<CompiledName("StateSerializer")>]
    let stateSerializer (state:MyModel) =
        state |> Json.serialize |> Json.format

    [<CompiledName("StateDeserializer")>]
    let stateDeserializer str : MyModel =
        str |> Json.parse |> Json.deserialize

[<AbstractClass>]
type MyAppComponent() =
    inherit Trail.ReduxComponent<MyModel, MyMsg>()
