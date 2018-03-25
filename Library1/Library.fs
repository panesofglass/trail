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
    open Newtonsoft.Json

    type WeatherForecast () =
        member val Date = Unchecked.defaultof<DateTime> with get, set
        member val TemperatureC = Unchecked.defaultof<int> with get, set
        member val TemperatureF = Unchecked.defaultof<int> with get, set
        member val Summary = Unchecked.defaultof<string> with get, set

    [<CompiledName("FetchForecastsAsync")>]
    let fetchForecasts (http:HttpClient) =
        async {
            let! response = Async.AwaitTask <| http.GetAsync("/sample-data/weather.json")
            let! json = Async.AwaitTask <| response.Content.ReadAsStringAsync()
            let forecasts = JsonConvert.DeserializeObject<WeatherForecast[]>(json)
            return forecasts
        }
        |> Async.StartAsTask
