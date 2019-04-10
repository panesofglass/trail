module Program

open Microsoft.AspNetCore.Blazor.Hosting

[<EntryPoint>]
let main args =
    BlazorApp1.Startup.CreateHostBuilder().Build().Run()
    0
