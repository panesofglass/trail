namespace BlazorHosted1.Client

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
    open BlazorHosted1.Shared

    [<CompiledName("FetchForecastsAsync")>]
    let fetchForecasts (http:HttpClient) =
        async {
            return! Async.AwaitTask <| http.GetJsonAsync<WeatherForecast[]>("/api/SampleData/WeatherForecasts")
        }
        |> Async.StartAsTask
