namespace FSharp.Blazor

open System
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components

module Dom =
    type Attribute =
        | HtmlAttribute of name:string * value:string
        | BlazorObjAttribute of name:string * value:obj
        | BlazorFragmentAttribute of name:string * (RenderTree.RenderTreeBuilder -> unit)

    type Node =
        | Element of name:string * attrs:Attribute list * nodes:Node list
        | Component of Type * attrs:Attribute list
        | Content of RenderFragment
        | Text of text:string
        | Fragment of Node list

    let comp<'T when 'T :> IComponent> attrs =
        Component(typeof<'T>, attrs)

    let el name attrs nodes = Element(name, attrs, nodes)
    let a attrs nodes = Element("a", attrs, nodes)
    let button attrs nodes = Element("button", attrs, nodes)
    let div attrs nodes = Element("div", attrs, nodes)
    let em attrs nodes = Element("em", attrs, nodes)
    let h1 attrs nodes = Element("h1", attrs, nodes)
    let h2 attrs nodes = Element("h2", attrs, nodes)
    let h3 attrs nodes = Element("h3", attrs, nodes)
    let h4 attrs nodes = Element("h4", attrs, nodes)
    let h5 attrs nodes = Element("h5", attrs, nodes)
    let p attrs nodes = Element("p", attrs, nodes)
    let span attrs nodes = Element("span", attrs, nodes)
    let strong attrs nodes = Element("strong", attrs, nodes)

    let ul attrs nodes = Element("ul", attrs, nodes)
    let li attrs nodes = Element("li", attrs, nodes)

    let table attrs nodes = Element("table", attrs, nodes)
    let thead attrs nodes = Element("thead", attrs, nodes)
    let tbody attrs nodes = Element("tbody", attrs, nodes)
    let tfoot attrs nodes = Element("tfoot", attrs, nodes)
    let tr attrs nodes = Element("tr", attrs, nodes)
    let th attrs nodes = Element("th", attrs, nodes)
    let td attrs nodes = Element("td", attrs, nodes)

    let content renderFragment = Content renderFragment
    
    let text content = Text content
    let textf format content = Text(Printf.sprintf format content)
    

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
            for attr in attrs do
                match attr with
                | HtmlAttribute(name, value) ->
                    printfn "AddAttribute(%i, %s, %s)" step name value
                    builder.AddAttribute(step, name, value)
                | BlazorObjAttribute(name, value) ->
                    printfn "AddAttribute(%i, %s, %A)" step name value
                    builder.AddAttribute(step, name, value)
                | BlazorFragmentAttribute(name, value) ->
                    printfn "AddAttribute(%i, %s, %A)" step name value
                    builder.AddAttribute(step, name, (RenderFragment(value)))
                step <- step + 1
            match nodes with
            | [] -> closeElement builder next cont step
            | _ -> renderNodes builder nodes step (closeElement builder next cont)
        | Component(ty, attrs) ->
            printfn "OpenComponent(%i, %s)" step (ty.Name)
            builder.OpenComponent(step, ty)
            step <- step + 1
            for attr in attrs do
                match attr with
                | HtmlAttribute(name, value) ->
                    printfn "AddAttribute(%i, %s, %s)" step name value
                    builder.AddAttribute(step, name, value)
                | BlazorObjAttribute(name, value) ->
                    printfn "AddAttribute(%i, %s, %A)" step name value
                    builder.AddAttribute(step, name, value)
                | BlazorFragmentAttribute(name, value) ->
                    printfn "AddAttribute(%i, %s, %A)" step name value
                    builder.AddAttribute(step, name, (RenderFragment(value)))
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
