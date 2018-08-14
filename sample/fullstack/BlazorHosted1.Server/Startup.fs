namespace BlazorHosted1.Server

open Microsoft.AspNetCore.Blazor.Server
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.ResponseCompression
open Microsoft.Extensions.DependencyInjection
open Newtonsoft.Json.Serialization
open System.Linq
open System.Net.Mime
open BlazorHosted1

type Startup() =

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc() |> ignore

        services.AddResponseCompression(fun options ->
            options.MimeTypes <-
                ResponseCompressionDefaults.MimeTypes.Concat(
                    [|
                        MediaTypeNames.Application.Octet
                        WasmMediaTypeNames.Application.Wasm
                    |])
        ) |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app.UseResponseCompression() |> ignore

        if env.IsDevelopment() then 
            app.UseDeveloperExceptionPage() |> ignore

        app.UseMvc(fun routes ->
            routes.MapRoute(name = "default", template = "{controller}/{action}/{id?}") |> ignore
        ) |> ignore

        app.UseBlazor<Client.Program>() |> ignore
