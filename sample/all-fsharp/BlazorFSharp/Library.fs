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
