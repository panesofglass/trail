namespace BlazorHosted1.Shared

open System

type WeatherForecast () =
    member val Date = Unchecked.defaultof<DateTime> with get, set
    member val TemperatureC = Unchecked.defaultof<int> with get, set
    member val TemperatureF = Unchecked.defaultof<int> with get, set
    member val Summary = Unchecked.defaultof<string> with get, set
