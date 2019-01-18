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
    open System.Threading.Tasks

    type WeatherForecast () =
        member val Date = Unchecked.defaultof<DateTime> with get, set
        member val TemperatureC = Unchecked.defaultof<int> with get, set
        member val TemperatureF = Unchecked.defaultof<int> with get, set
        member val Summary = Unchecked.defaultof<string> with get, set

    let summaries = [|"Freezing";"Bracing";"Chilly";"Cool";"Mild";"Warm";"Balmy";"Hot";"Sweltering";"Scorching"|]

    let degCtoF x = x * 9 / 5 + 32

    type IForecastService =
        abstract FetchForecastsAsync : startDate:DateTime -> Task<WeatherForecast[]>

    let forecastService =
        { new IForecastService with
            member this.FetchForecastsAsync(startDate:DateTime) =
                let rng = Random()
                [| for i in 1..5 ->
                    let tempC = rng.Next(-20, 55)
                    WeatherForecast(
                        Date = startDate.AddDays(float i),
                        TemperatureC = tempC,
                        TemperatureF = degCtoF tempC,
                        Summary = summaries.[rng.Next(summaries.Length)])
                |]
                |> Task.FromResult
        }
        
    type WeatherForecastService() =
        interface IForecastService with
            member this.FetchForecastsAsync(startDate:DateTime) =
                let rng = Random()
                [| for i in 1..5 ->
                    let tempC = rng.Next(-20, 55)
                    WeatherForecast(
                        Date = startDate.AddDays(float i),
                        TemperatureC = tempC,
                        TemperatureF = degCtoF tempC,
                        Summary = summaries.[rng.Next(summaries.Length)])
                |]
                |> Task.FromResult
