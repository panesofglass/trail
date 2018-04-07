# Trail

[Blazor](https://github.com/aspnet/Blazor) rendering with [F#](http://fsharp.org/).

[![NuGet Status](http://img.shields.io/nuget/v/trail.svg?style=flat)](https://www.nuget.org/packages/trail/)
[![Issue Stats](http://issuestats.com/github/panesofglass/trail/badge/issue)](http://issuestats.com/github/panesofglass/trail)
[![PR Stats](http://issuestats.com/github/panesofglass/trail/badge/pr)](http://issuestats.com/github/panesofglass/trail)

## Features

* F# running in the browser on WebAssembly!
* Domain-specific language for Blazor components similar to those provided by [Fable](http://fable.io/) and [WebSharper](http://websharper.com/).
* `Trail.Component` type to make it easier to create components with the DSL.
* [Sample application](https://github.com/panesofglass/trail/tree/master/sample) demonstrating the capabilities based on the stand-alone Blazor template.
* Can write nearly the entire app in F#! All you need is a `csproj` for the `Program.cs` and assets.

## Getting Started

1. Follow the instructions for [getting started with Blazor](https://github.com/aspnet/Blazor#getting-started).
2. Clone this repository and run `dotnet run --project sample/BlazorApp1` or run from the latest Visual Studio 2017 Preview.
3. You can also create a new Blazor project,
4. Add an F# library project,
5. Add the following libraries to the F# library project:
``` xml
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Blazor.Browser" Version="0.1.0" PrivateAssets="all" />
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

type Marker = class end

type App() =
    inherit Trail.Component()

    override __.Render() =
        Dom.comp<Router> [Dom.BlazorObjAttribute("AppAssembly", box typeof<Marker>.Assembly)] []
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

> Aside:
> You may be wondering, "What's with all the namespaces?" The namespaces are repeated because, at least in my experiments, I found that the web assembly compiler seems to need those opened in order to compile properly. We may be able to reduce them a bit now that it's all in F#.

### Shared

There are several shared components, including the navigation menu, the main layout, and the survey. You can find these in the `Shared.fs` file in the [sample folder](https://github.com/panesofglass/trail/blob/master/sample/Library1/Shared.fs).

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

## Roadmap

1. More documentation, samples, and tutorials!
2. Test and optimize performance
3. Keep up with ASP.NET Blazor team
4. All F# Blazor app - doesn't seem possible yet.

## Contributing

Trail is very early and building on top of Blazor, which is also very early. Expect many breaking changes to come.
I would love to have your help. If you have ideas, run into issues, or want to tweak or extend the DSL, please
submit [issues](https://github.com/panesofglass/trail/issues) or [pull requests](https://github.com/panesofglass/trail/pulls).
