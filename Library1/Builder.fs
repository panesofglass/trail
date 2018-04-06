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
        printfn "Received %i nodes" nodes.Length
        buildNodes builder nodes 0 ignore
    and private buildNodes (builder:RenderTree.RenderTreeBuilder) (nodes:Node list) sequence cont =
        match nodes with
        | [] ->
            printfn "Calling cont %A with sequence %i" cont sequence
            cont sequence
        | node::nodes ->
            printfn "building node %A at sequence %i" node sequence
            buildNode builder nodes node sequence cont
    and private buildNode (builder:RenderTree.RenderTreeBuilder) next node sequence cont =
        let mutable step = sequence
        match node with
        | Element(name, attrs, nodes) ->
            printfn "Opening a new %s element at sequence %i" name step
            builder.OpenElement(step, name)
            step <- step + 1
            for Attribute(name, value) in attrs do
                printfn "Adding the %s attribute at sequence %i" name step
                builder.AddAttribute(step, name, value)
                step <- step + 1
            match nodes with
            | [] -> closeElement builder next step
            | _ -> buildNodes builder nodes step (closeElement builder next)
        | Component(ty, attrs) ->
            printfn "Opening a new %s component at sequence %i" (ty.Name) step
            builder.OpenComponent(step, ty)
            step <- step + 1
            for Attribute(name, value) in attrs do
                printfn "Adding the %s attribute at sequence %i" name step
                builder.AddAttribute(step, name, value)
                step <- step + 1
            printfn "Closing the component at step %i" step
            builder.CloseComponent()
            builder.AddContent(step, "\n")
            step <- step + 1
            buildNodes builder next step cont
        | Content(text) ->
            printfn """Adding content "%s" at sequence %i""" text step
            builder.AddContent(step, text)
            buildNodes builder next (step + 1) cont
    and private closeElement (builder:RenderTree.RenderTreeBuilder) next sequence =
        printfn "Closing the element at step %i" sequence
        builder.CloseElement()
        builder.AddContent(sequence, "\n")
        buildNodes builder next (sequence + 1) ignore
