namespace FSharp.Blazor

open System
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components

module Dom =
    type Attribute =
        Attribute of name:string * value:string
        with
        member this.Name = let (Attribute(name, _)) = this in name
        member this.Value = let (Attribute(_, value)) = this in value

    type Node =
        | Element of name:string * attrs:Attribute list * nodes:Node list
        | Component of Type * attrs:Attribute list
        | Content of RenderFragment
        | Text of text:string
        | Fragment of Node list

    let comp<'T when 'T :> IComponent> attrs =
        Component(typeof<'T>, attrs |> List.map Attribute)

    let el name attrs nodes =
        Element(name, attrs |> List.map Attribute, nodes)
    
    let a attrs nodes =
        Element("a", attrs |> List.map Attribute, nodes)
    
    let div attrs nodes =
        Element("div", attrs |> List.map Attribute, nodes)

    let em attrs nodes =
        Element("em", attrs |> List.map Attribute, nodes)
    
    let h1 attrs nodes =
        Element("h1", attrs |> List.map Attribute, nodes)
    
    let span attrs nodes =
        Element("span", attrs |> List.map Attribute, nodes)

    let strong attrs nodes =
        Element("strong", attrs |> List.map Attribute, nodes)

    let content renderFragment =
        Content renderFragment
    
    let text content =
        Text content
    

module RenderTree =
    open Dom

    let rec render builder node =
        match node with
        | Fragment nodes ->
            renderNodes builder nodes 0 ignore
        | _ -> renderNode builder [] node 0 ignore
    and private renderNodes (builder:RenderTree.RenderTreeBuilder) (nodes:Node list) sequence cont =
        match nodes with
        | [] ->
            cont sequence
        | node::nodes ->
            renderNode builder nodes node sequence cont
    and private renderNode (builder:RenderTree.RenderTreeBuilder) next node sequence cont =
        let mutable step = sequence
        match node with
        | Element(name, attrs, nodes) ->
            printfn "OpenElement(%i, %s)" step name
            builder.OpenElement(step, name)
            step <- step + 1
            for Attribute(name, value) in attrs do
                printfn "AddAttribute(%i, %s, %s)" step name value
                builder.AddAttribute(step, name, value)
                step <- step + 1
            match nodes with
            | [] -> closeElement builder next cont step
            | _ -> renderNodes builder nodes step (closeElement builder next cont)
        | Component(ty, attrs) ->
            printfn "OpenComponent(%i, %s)" step (ty.Name)
            builder.OpenComponent(step, ty)
            step <- step + 1
            for Attribute(name, value) in attrs do
                printfn "AddAttribute(%i, %s, %s)" step name value
                builder.AddAttribute(step, name, value)
                step <- step + 1
            printfn "CloseComponent()"
            builder.CloseComponent()
            printfn "AddContent(%i, \\n)" step
            builder.AddContent(step, "\n")
            step <- step + 1
            renderNodes builder next step cont
        | Content(renderFragment) ->
            printfn "AddContent(%i, %A)" step renderFragment
            builder.AddContent(step, renderFragment)
            renderNodes builder next (step + 1) cont
        | Text(text) ->
            printfn "AddContent(%i, %s)" step text
            builder.AddContent(step, text)
            renderNodes builder next (step + 1) cont
        | Fragment(nodes) ->
            renderNodes builder nodes step cont
    and private closeElement (builder:RenderTree.RenderTreeBuilder) next cont sequence =
        printfn "CloseElement()"
        builder.CloseElement()
        printfn "AddContent(%i, \\n)" sequence
        builder.AddContent(sequence, "\n")
        renderNodes builder next (sequence + 1) cont
    

module Extensions =
    type Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder with
        /// Renders a Dom.Document.
        member this.Render(document) = RenderTree.render this document
