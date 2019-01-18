namespace BlazorApp1.FSharp.Server

open System.Net.Mime
open Microsoft.AspNetCore.Blazor.Server
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.ResponseCompression
open Microsoft.Extensions.DependencyInjection

type Startup() =

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddServerSideBlazor<Startup>() |> ignore
        services.AddResponseCompression(fun options ->
            let mimeTypes = seq {
                yield! ResponseCompressionDefaults.MimeTypes
                yield MediaTypeNames.Application.Octet
                yield WasmMediaTypeNames.Application.Wasm
            }
            options.MimeTypes <- mimeTypes
        ) |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app.UseResponseCompression() |> ignore

        if env.IsDevelopment() then 
            app.UseDeveloperExceptionPage() |> ignore

        app.UseServerSideBlazor<Startup>() |> ignore
