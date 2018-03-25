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
    open System.Threading.Tasks
    open Newtonsoft.Json

    type WeatherForecast () =
        member val Date = Unchecked.defaultof<DateTime> with get, set
        member val TemperatureC = Unchecked.defaultof<int> with get, set
        member val TemperatureF = Unchecked.defaultof<int> with get, set
        member val Summary = Unchecked.defaultof<string> with get, set

    [<CompiledName("FetchForecastsAsync")>]
    let fetchForecasts (http:HttpClient) =
        (*
        async {
            let! json = Async.AwaitTask <| http.GetStringAsync("/sample-data/weather.json")
            return JsonConvert.DeserializeObject<WeatherForecast[]>(json)
        }
        |> Async.StartAsTask
        *)
        http.GetStringAsync("/sample-data/weather.json").ContinueWith(Func<Task<string>,WeatherForecast[]>(fun task ->
            JsonConvert.DeserializeObject<WeatherForecast[]>(task.Result)))
