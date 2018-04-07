# Trail

[Blazor](https://github.com/aspnet/Blazor) rendering with [F#](http://fsharp.org/).

## Features

* Domain-specific language for Blazor components similar to those provided by [Fable](http://fable.io/) and [WebSharper](http://websharper.com/).
* `Trail.Component` type to make it easier to create components with the DSL.
* [Sample application](https://github.com/panesofglass/trail/tree/master/sample) demonstrating the capabilities based on the stand-alone Blazor template.

## Getting Started

1. Follow the instructions for [getting started with Blazor](https://github.com/aspnet/Blazor#getting-started).
2. Create a new Blazor project.
3. Add an F# library project.
4. Add the following libraries to the F# library project:
``` xml
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Blazor.Browser" Version="0.1.0" PrivateAssets="all" />
    <PackageReference Include="Trail" Version="0.*" />
  </ItemGroup>
```
5. Add `Trail.Component`s to the library.

## Roadmap

1. More documentation, samples, and tutorials!
2. Test and optimize performance
3. Keep up with ASP.NET Blazor team
