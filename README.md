# Trail

[Blazor](https://github.com/aspnet/Blazor) rendering with [F#](http://fsharp.org/).

[![Build Status](https://travis-ci.org/panesofglass/trail.svg?branch=master)](https://travis-ci.org/panesofglass/trail)

## UPDATE Jan 18, 2019

Thank you to everyone who showed interest in this project. For those looking for updates, please see the [Bolero](https://github.com/fsbolero/Bolero) project, which has already completed and surpassed my goals for trail.
I do not currently have any plans to continue working on this project and recommend all interested parties to contribute to [Bolero](https://github.com/fsbolero/Bolero) instead.

## Packages

| Package | NuGet | Downloads |
| :------ | :-----: | :-------: |
| Trail | [![NuGet Status](http://img.shields.io/nuget/v/Trail.svg?style=flat)](https://www.nuget.org/packages/Trail/) | ![NuGet Downloads](https://img.shields.io/nuget/dt/Trail.svg?style=flat) |
| Trail.BlazorRedux | [![NuGet Status](http://img.shields.io/nuget/v/Trail.BlazorRedux.svg?style=flat)](https://www.nuget.org/packages/Trail.BlazorRedux/) | ![NuGet Downloads](https://img.shields.io/nuget/dt/Trail.BlazorRedux.svg?style=flat) |

## Features

* F# running in the browser on WebAssembly!
* Domain-specific language for Blazor components similar to those provided by [Fable](http://fable.io/) and [WebSharper](http://websharper.com/).
* `Trail.Component` type to make it easier to create components with the DSL.
* [Sample application](https://github.com/panesofglass/trail/tree/master/sample/standalone) demonstrating the capabilities based on the stand-alone Blazor template.
* Can write nearly the entire app in F#! All you need is a `csproj` for the [`Program.cs` and assets](https://github.com/panesofglass/trail/tree/master/sample/standalone/BlazorApp1).

## Getting Started

1. Follow the instructions for [getting started with Blazor](https://github.com/aspnet/Blazor#getting-started).
2. Clone this repository and run `dotnet run --project sample/standalone/BlazorApp1` or run from the latest Visual Studio 2017 Preview.
3. You can also create a new Blazor project,
4. Add an F# library project,
5. Add the following libraries to the F# library project:
``` xml
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Blazor.Browser" Version="0.5.1" />
    <PackageReference Include="Trail" Version="0.*" />
  </ItemGroup>
```
, and
5. Begin adding `Trail.Component`s to the library. See below.

### App

An application needs a router to discover and provide navigation to all the pages. An `App` component may look like the following in Trail:

``` fsharp
namespace BlazorApp1

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Browser.Rendering
open Microsoft.AspNetCore.Blazor.Browser.Services
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open Microsoft.AspNetCore.Blazor.Routing
open BlazorApp1
open BlazorApp1.Shared
open Trail

type App() =
    inherit Trail.Component()

    override __.Render() =
        Dom.router<Router> typeof<App>.Assembly
```

A `Trail.Component` is an abstract class inheriting from a `BlazorComponent`.
It expects an implementation of the `Render` method and handles the rest for you.

### Index

The sample app provides several pages, including the index page, which just has some text and a custom Blazor component. The code for the index page looks like this:

``` fsharp
namespace BlazorApp1.Pages

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open Microsoft.AspNetCore.Blazor.Routing
open BlazorApp1
open BlazorApp1.Shared
open Trail

[<LayoutAttribute(typeof<MainLayout>)>]
[<RouteAttribute("/")>]
type Index () =
    inherit Trail.Component()

    override __.Render() =
        Dom.Fragment [
            Dom.h1 [] [ Dom.text "Hello, world!" ]
            Dom.text "\n\nWelcome to your new app.\n\n"
            Dom.comp<SurveyPrompt> [Dom.HtmlAttribute("title", "How is Blazor working for you?")] []
        ]
```

This is a bit more involved, as it includes several DOM nodes, as well as several attributes. These attributes identify this component as a page component. Child components will not require these attributes. (Refer to the [Blazor repo](https://github.com/aspnet/Blazor) for more accurate information, as this is still rapidly changing.)

The attributes are Blazor attributes for routing and applying a layout. (We'll look at the `MainLayout` next.) As you can see, the `RouteAttribute` specifies that the index page should be found at the site root (naturally).

The `Render` method is then implemented with a `Dom.Fragment`. This keeps things simple on the processing side, as we know we'll always have all the elements wrapped up in a single node. The `Dom.Fragment` is not rendered in any way and works very similar to the fragment support in React.

Finally, you can see Trail already provides several helper elements, e.g. `h1`, `text`, and `comp<'T>`. What's a `comp<'T>`? As you may suspect, this is a way of rendering a custom Blazor component. We'll look at the `SurveyPrompt` after the `MainLayout`.

### Shared

There are several shared components, including the navigation menu, the main layout, and the survey. You can find these in the `Shared.fs` file in the [sample folder](https://github.com/panesofglass/trail/blob/master/sample/standalone/Library1/Shared.fs).

``` fsharp
type MainLayout () =
    inherit Trail.Component()

    override this.Render() =
        Dom.div [Dom.HtmlAttribute("class", "container-fluid")] [
            Dom.div [Dom.HtmlAttribute("class", "row")] [
                Dom.div [Dom.HtmlAttribute("class", "col-sm-3")] [
                    Dom.comp<NavMenu> [] []
                ]
                Dom.div [Dom.HtmlAttribute("class", "col-sm-9")] [
                    Dom.content this.Body
                ]
            ]
        ]

    member val Body : RenderFragment = Unchecked.defaultof<RenderFragment> with get, set

    interface ILayoutComponent with
        member this.Body with get() = this.Body
                          and set(value) = this.Body <- value
```

This is the `MainLayout`. You can see that it uses the `NavMenu` component and has a `Body` typed as a `RenderFragment`. Your page component is rendered in the `Body` of the `Layout` as defined by the use of the `ILayoutComponent` interface.

``` fsharp
type SurveyPrompt () =
    inherit Trail.Component()

    override this.Render() =
        Dom.div [Dom.HtmlAttribute("class", "alert alert-survey"); Dom.HtmlAttribute("role", "alert")] [
            Dom.span [Dom.HtmlAttribute("class", "glyphicon glyphicon-ok-circle"); Dom.HtmlAttribute("aria-hidden", "true")] []
            Dom.strong [] [Dom.text this.Title]
            Dom.text "Please take our "
            Dom.a [Dom.HtmlAttribute("target", "_blank"); Dom.HtmlAttribute("class", "alert-link"); Dom.HtmlAttribute("href", "https://go.microsoft.com/fwlink/?linkid=870381")] [
                Dom.text "brief survey"
            ]
            Dom.text " and tell us what you think."
        ]

    // This is to demonstrate how a parent component can supply parameters
    member val Title : string = Unchecked.defaultof<string> with get, set
```

The `SurveyPrompt` component doesn't have any attributes or special interfaces. However, it does provide a property that may be filled in where it is used. Be careful, however, with casing. Here's where we specified the `Title` property above:

``` fsharp
Dom.comp<SurveyPrompt> [Dom.HtmlAttribute("title", "How is Blazor working for you?")] []
```

Note that the attribute name is lowercase. This is an area where I think we can improve type-safety with the DSL.

### Blazor-Redux

Trail provids [Blazor-Redux](https://github.com/torhovland/blazor-redux) component integration, as well.
Follow the instructions in the [Blazor-Redux README](https://github.com/torhovland/blazor-redux/blob/master/README.md)
to learn how to use that library. The primary change to use Trail is to convert your `Trail.Component` into a
`Trail.ReduxComponent`. You will need to create a base component for your application,
just as in the Blazor-Redux example, only it should be a `Trail.ReduxComponent`:

``` fsharp
[<AbstractClass>]
type MyAppComponent() =
    inherit Trail.ReduxComponent<MyModel, MyMsg>()
```

Note that this component has an `[<AbstractClass>]` attribute to indicate that it must be implemented. This is to avoid
having to provide an implementation of the `Render` member.

#### Counter

The `Counter` component looks much like the one above, only you need to call `this.Dispatch` to dispatch the action,
rather than handling directly within the component:

``` fsharp
[<Layout(typeof<MainLayout>)>]
[<Route("/counter")>]
type Counter () =
    inherit MyAppComponent()

    override this.Render() =
        Dom.Fragment [
            Dom.h1 [] [Dom.text "Counter"]
            Dom.p [] [
                Dom.text "Current count: "
                Dom.textf "%i" this.State.Count
            ]
            Dom.button [
                    Attr.onclick(fun _ -> this.Dispatch(MyMsg.IncrementByOne))
                ] [
                    Dom.text "Click me"
                ]
        ]
```

#### FetchData

The `FetchData` component is also very similar to the standard `FetchData` component above:

``` fsharp
[<Layout(typeof<MainLayout>)>]
[<Route("/fetchdata")>]
type FetchData () =
    inherit MyAppComponent()

    override this.Render() =
        Dom.Fragment [
            yield Dom.h1 [] [Dom.text "Weather forecast"]
            yield Dom.p [] [Dom.text "This component domonstrates fetching data from the server."]
            match this.State.Forecasts with
            | None | Some [||] ->
                yield Dom.p [] [
                    Dom.em [] [Dom.text "Loading..."]
                ]
            | Some forecasts ->
                yield Dom.table [Dom.HtmlAttribute("class", "table")] [
                    Dom.thead [] [
                        Dom.tr [] [
                            Dom.th [] [Dom.text "Date"]
                            Dom.th [] [Dom.text "Temp. (C)"]
                            Dom.th [] [Dom.text "Temp. (F)"]
                            Dom.th [] [Dom.text "Summary"]
                        ]
                    ]
                    Dom.tbody [] [
                        for forecast in forecasts ->
                            Dom.tr [] [
                                Dom.td [] [Dom.text (forecast.Date.ToShortDateString())]
                                Dom.td [] [Dom.textf "%i" forecast.TemperatureC]
                                Dom.td [] [Dom.textf "%i" forecast.TemperatureF]
                                Dom.td [] [Dom.text forecast.Summary]
                            ]
                    ]
                ]
        ]

    override this.OnInitAsync() =
        ActionCreators.LoadWeather(BlazorRedux.Dispatcher this.Store.Dispatch, this.Http)
    
    [<Inject>]
    member val private Http : HttpClient = Unchecked.defaultof<HttpClient> with get, set
```

Here, we use the `ActionCreators.LoadWeather`, as seen in the [Blazor-Redux example](https://github.com/torhovland/blazor-redux/blob/master/README.md).
You must specify a `BlazorRedux.Dispatcher` delegate using `this.Store.Dispatch` method from the
`Trail.ReduxComponent`, as well as the injected `this.Http` `HttpClient` instance.

#### ReduxDevTools

Blazor-Redux integrates with the [Redux DevTools](https://github.com/gaearon/redux-devtools), and you can add this
integration to your `Trail.BlazorRedux` app by rendering the `BlazorRedux.ReduxDevTools` component. In the sample,
I've added the component to the `App`:

``` fsharp
type App() =
    inherit MyAppComponent()

    override __.Render() =
        Dom.Fragment [
            Dom.router<Router> typeof<App>.Assembly
            Dom.comp<BlazorRedux.ReduxDevTools> [] []
        ]
```

You can find the [stand-alone sample here](https://github.com/panesofglass/trail/tree/master/sample/blazor-redux-standalone/).
NOTE: this README does not cover the creation of the `MyModel`, `MyMsg`, or the reducer types and function. For those details,
see the [sample above](https://github.com/panesofglass/trail/tree/master/sample/blazor-redux-standalone/) or the
[Blazor-Redux README](https://github.com/torhovland/blazor-redux/blob/master/README.md).

## Roadmap

- [ ] More documentation, samples, and tutorials!
	- [x] Sample with [Blazor-Redux](https://github.com/torhovland/blazor-redux)
	- [x] [Full-stack sample](https://github.com/panesofglass/trail/issues/2)
	- [ ] [Full-stack sample with Blazor-Redux](https://github.com/panesofglass/trail/issues/2)
	- [ ] [Realworld.io](https://realworld.io/) [sample](https://github.com/panesofglass/trail/issues/4)
- [ ] [Extend and improve markup helper DSL](https://github.com/panesofglass/trail/issues/5)
- [ ] [Test and optimize performance](https://github.com/panesofglass/trail/issues/6)
- [ ] [Create dotnet new templates](https://github.com/panesofglass/trail/issues/7)
- [ ] Keep up with ASP.NET Blazor team
- [ ] All F# Blazor app - doesn't seem possible yet.

## Contributing

Trail is very early and building on top of Blazor, which is also very early. Expect many breaking changes to come.
I would love to have your help. If you have ideas, run into issues, or want to tweak or extend the DSL, please
submit [issues](https://github.com/panesofglass/trail/issues) or [pull requests](https://github.com/panesofglass/trail/pulls).
