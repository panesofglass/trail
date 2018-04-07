namespace Trail

open System
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components

module Dom =
    type Attribute =
        | HtmlAttribute of name:string * value:string
        | BlazorFrameAttribute of frame:RenderTree.RenderTreeFrame
        | BlazorObjAttribute of name:string * value:obj

    type Node =
        | Element of name:string * attrs:Attribute list * children:Node list
        | Component of Type * attrs:Attribute list * children:Node list
        | Content of RenderFragment
        | Text of text:string
        | Fragment of Node list

    let comp<'T when 'T :> IComponent> attrs children =
        Component(typeof<'T>, attrs, children)

    let el name attrs children = Element(name, attrs, children)
    let a attrs children = Element("a", attrs, children)
    let button attrs children = Element("button", attrs, children)
    let div attrs children = Element("div", attrs, children)
    let em attrs children = Element("em", attrs, children)
    let h1 attrs children = Element("h1", attrs, children)
    let h2 attrs children = Element("h2", attrs, children)
    let h3 attrs children = Element("h3", attrs, children)
    let h4 attrs children = Element("h4", attrs, children)
    let h5 attrs children = Element("h5", attrs, children)
    let p attrs children = Element("p", attrs, children)
    let span attrs children = Element("span", attrs, children)
    let strong attrs children = Element("strong", attrs, children)

    let ul attrs children = Element("ul", attrs, children)
    let li attrs children = Element("li", attrs, children)

    let table attrs children = Element("table", attrs, children)
    let thead attrs children = Element("thead", attrs, children)
    let tbody attrs children = Element("tbody", attrs, children)
    let tfoot attrs children = Element("tfoot", attrs, children)
    let tr attrs children = Element("tr", attrs, children)
    let th attrs children = Element("th", attrs, children)
    let td attrs children = Element("td", attrs, children)

    let content fragment = Content fragment
    
    let text textContent = Text textContent
    let textf format content = Text(Printf.sprintf format content)
    
module RenderTree =
    open Dom

    type AST =
        | OpenElement of sequence:int * name:string
        | CloseElement
        | OpenComponent of sequence:int * ty:Type
        | CloseComponent
        | AddTextContent of sequence:int * textContent:string
        | AddRenderFragmentContent of sequence:int * fragment:RenderFragment
        | AddBlazorFragmentAttribute of sequence:int * fragment:AST list
        | AddBlazorFrameAttribute of sequence:int * frame:RenderTree.RenderTreeFrame
        | AddBlazorObjAttribute of sequence:int * name:string * value:obj
        | AddHtmlAttribute of sequence:int * name:string * value:string

    let rec build node =
        match node with
        | Fragment nodes ->
            buildNodes nodes 0 id
        | _ -> buildNode [] node 0 id
        |> snd

    and private buildNodes (nodes:Node list) sequence cont =
        match nodes with
        | [] ->
            cont(sequence, [])
        | node::nodes ->
            buildNode nodes node sequence cont

    and private buildNode next node sequence cont =
        let mutable step = sequence
        match node with
        | Element(name, attrs, children) ->
            let instructions =
                [
                    yield OpenElement(step, name)
                    step <- step + 1
                    for attr in attrs do
                        match attr with
                        | HtmlAttribute(name, value) ->
                            yield AddHtmlAttribute(step, name, value)
                        | BlazorFrameAttribute(frame) ->
                            yield AddBlazorFrameAttribute(step, frame)
                        | BlazorObjAttribute(name, value) ->
                            yield AddBlazorObjAttribute(step, name, value)
                        step <- step + 1
                ]
            match children with
            | [] -> closeElement next (fun (sequence', rest) -> cont(sequence', instructions @ rest)) step
            | _ ->
                // TODO: Correctly nest closeElement within the continuation.
                let sequence', children = buildNodes children step id
                closeElement next (fun (sequence'', rest) -> cont(sequence'', instructions @ children @ rest)) sequence'

        | Component(ty, attrs, children) ->
            let instructions =
                [
                    yield OpenComponent(step, ty)
                    step <- step + 1
                    for attr in attrs do
                        match attr with
                        | HtmlAttribute(name, value) ->
                            yield AddHtmlAttribute(step, name, value)
                        | BlazorFrameAttribute(frame) ->
                            yield AddBlazorFrameAttribute(step, frame)
                        | BlazorObjAttribute(name, value) ->
                            yield AddBlazorObjAttribute(step, name, value)
                        step <- step + 1
                ]
            match children with
            | [] -> closeComponent next (fun (sequence', rest) -> cont(sequence', instructions @ rest)) step
            | _ ->
                // TODO: Correctly nest createing the fragment attribute and closing the component within the continuation.
                let sequence', children = buildNodes children (step + 1) id
                let fragment = AddBlazorFragmentAttribute(step, children)
                closeComponent next (fun (sequence'', rest) -> cont(sequence'', instructions @ [fragment] @ rest)) sequence'

        | Content(fragment) ->
            buildNodes next (step + 1) (fun (sequence', rest) -> cont(sequence', [AddRenderFragmentContent(step, fragment)] @ rest))

        | Text(textContent) ->
            buildNodes next (step + 1) (fun (sequence', rest) -> cont(sequence', [AddTextContent(step, textContent)] @ rest))

        | Fragment(nodes) ->
            // TODO: Correctly nest building fragment nodes within the continuation.
            let sequence', instructions = buildNodes nodes step id
            buildNodes next sequence' (fun (sequence'', rest) -> cont(sequence'', instructions @ rest))

    and private closeElement next cont sequence =
        let instructions =
            [
                CloseElement
                AddTextContent(sequence, "\n")
            ]
        buildNodes next (sequence + 1) (fun (sequence', rest) -> cont (sequence', instructions @ rest))

    and private closeComponent next cont sequence =
        let instructions =
            [
                CloseComponent
                AddTextContent(sequence, "\n")
            ]
        buildNodes next (sequence + 1) (fun (sequence', rest) -> cont (sequence', instructions @ rest))

    let rec render (builder:RenderTree.RenderTreeBuilder) instructions =
        for instruction in instructions do
            match instruction with
            | OpenElement(sequence, name) ->
                builder.OpenElement(sequence, name)
            | CloseElement ->
                builder.CloseElement()
            | OpenComponent(sequence, ty) ->
                builder.OpenComponent(sequence, ty)
            | CloseComponent ->
                builder.CloseComponent()
            | AddTextContent(sequence, textContent) ->
                builder.AddContent(sequence, textContent)
            | AddRenderFragmentContent(sequence, fragment) ->
                builder.AddContent(sequence, fragment)
            | AddBlazorFragmentAttribute(sequence, fragment) ->
                builder.AddAttribute(sequence, "ChildContent", RenderFragment(fun builder2 -> render builder2 fragment))
            | AddBlazorFrameAttribute(sequence, frame) ->
                builder.AddAttribute(sequence, frame)
            | AddBlazorObjAttribute(sequence, name, value) ->
                builder.AddAttribute(sequence, name, value)
            | AddHtmlAttribute(sequence, name, value) ->
                builder.AddAttribute(sequence, name, value)

module Extensions =

    type Dom.Node with

        /// Compiles a Dom.Node into an AST tree.
        member this.Compile() =
            RenderTree.build this

    type Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder with
        
        /// Renders the compiled Dom.Node instructions.
        member this.Render(instructions) =
            instructions |> RenderTree.render this

        /// Renders a Dom.Node.
        member this.Render(document) =
            document
            |> RenderTree.build
            |> RenderTree.render this
