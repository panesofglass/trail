namespace BlazorApp1.FSharp.Server

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration

module Program =
    let exitCode = 0

    let CreateWebHostBuilder args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseConfiguration(ConfigurationBuilder().AddCommandLine(args).Build())
            .UseStartup<Startup>();            

    [<EntryPoint>]
    let main args =
        CreateWebHostBuilder(args).Build().Run()

        exitCode
