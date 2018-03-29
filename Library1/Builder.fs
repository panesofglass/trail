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
    
    type Document = Document of Node list

    let comp<'T when 'T :> IComponent> attrs =
        Component(typeof<'T>, attrs |> List.map Attribute)

    let el name attrs nodes =
        Element(name, attrs |> List.map Attribute, nodes)
    
    let text content =
        Content content
    

module RenderTree =
    open Dom

    let rec build builder (Document nodes) =
        buildNodes builder nodes 0 ignore
    and private buildNodes (builder:RenderTree.RenderTreeBuilder) (nodes:Node list) sequence cont =
        match nodes with
        | [] -> cont sequence
        | node::nodes -> buildNode builder nodes node sequence
    and private buildNode (builder:RenderTree.RenderTreeBuilder) next node sequence =
        let mutable step = sequence
        match node with
        | Element(name, attrs, nodes) ->
            builder.OpenElement(step, name)
            step <- step + 1
            for Attribute(name, value) in attrs do
                builder.AddAttribute(step, name, value)
                step <- step + 1
            buildNodes builder nodes step (fun sequence ->
                builder.CloseComponent()
                buildNodes builder next sequence ignore
            )
        | Component(ty, attrs) ->
            builder.OpenComponent(step, ty)
            step <- step + 1
            for Attribute(name, value) in attrs do
                builder.AddAttribute(step, name, value)
                step <- step + 1
            builder.CloseComponent()
            buildNodes builder next step ignore
        | Content(text) ->
            builder.AddContent(step, text)
            buildNodes builder next (step + 1) ignore
