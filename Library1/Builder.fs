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
        | Content of text:string
    
    type Document = Fragment of Node list

    let comp<'T when 'T :> IComponent> attrs =
        Component(typeof<'T>, attrs |> List.map Attribute)

    let el name attrs nodes =
        Element(name, attrs |> List.map Attribute, nodes)
    
    let text content =
        Content content
    

module RenderTree =
    open Dom

    let rec build builder (Fragment nodes) =
        buildNodes builder nodes 0 ignore
    and private buildNodes (builder:RenderTree.RenderTreeBuilder) (nodes:Node list) sequence cont =
        match nodes with
        | [] ->
            cont sequence
        | node::nodes ->
            buildNode builder nodes node sequence cont
    and private buildNode (builder:RenderTree.RenderTreeBuilder) next node sequence cont =
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
            | _ -> buildNodes builder nodes step (closeElement builder next cont)
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
            buildNodes builder next step cont
        | Content(text) ->
            printfn "AddContent(%i, %s)" step text
            builder.AddContent(step, text)
            buildNodes builder next (step + 1) cont
    and private closeElement (builder:RenderTree.RenderTreeBuilder) next cont sequence =
        printfn "CloseElement()"
        builder.CloseElement()
        printfn "AddContent(%i, \\n)" sequence
        builder.AddContent(sequence, "\n")
        buildNodes builder next (sequence + 1) cont
