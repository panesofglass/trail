namespace BlazorHosted1.Server.Controllers

open BlazorHosted1.Shared
open Microsoft.AspNetCore.Mvc
open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks

[<Route("api/[controller]")>]
type SampleDataController () =
    inherit Controller()

    let summaries =
        [|
            "Freezing"; "Bracing"; "Chilly"; "Cool"; "Mild"; "Warm"; "Balmy"; "Hot"; "Sweltering"; "Scorching"
        |]

    [<HttpGet("[action]")>]
    member this.WeatherForecasts() =
        let rng = Random()
        seq {
            for index in 1..5 ->
                WeatherForecast(
                    Date = DateTime.Now.AddDays(float index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = summaries.[rng.Next(summaries.Length)]
                )
        }
