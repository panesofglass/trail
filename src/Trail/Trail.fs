namespace Trail

open System
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Routing
open Microsoft.AspNetCore.Blazor.Layouts

module Dom =
    open System.Reflection

    type Attribute =
        | HtmlAttribute of name:string * value:string
        | HtmlConditionalAttribute of name:string * value:bool
        | BlazorEventHandlerAttribute of name:string * handler:MulticastDelegate
        | BlazorFrameAttribute of frame:RenderTree.RenderTreeFrame ref
        | BlazorObjAttribute of name:string * value:obj
        // TODO: Bind attribute?

    type Node =
        | Element of name:string * attrs:Attribute list * children:Node list
        | Component of Type * attrs:Attribute list * children:Node list
        | Content of RenderFragment
        | Text of text:string
        | Fragment of Node list

    let comp<'T when 'T :> IComponent> attrs children =
        Component(typeof<'T>, attrs, children)

    let router<'T when 'T :> Router> (assembly:Reflection.Assembly) =
        Component(typeof<'T>, [BlazorObjAttribute("AppAssembly", box assembly)], [])

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

    let input attrs = Element("input", attrs, [])

module Attr =
    open Dom

    let id value = HtmlAttribute("id", value)
    let className value = HtmlAttribute("class", value)
    let href value = HtmlAttribute("href", value)
    let isChecked value = HtmlConditionalAttribute("checked", value)
    let isDisabled value = HtmlConditionalAttribute("disabled", value)
    let name value = HtmlAttribute("name", value)
    let rel value = HtmlAttribute("rel", value)
    let typ value = HtmlAttribute("type", value)
    let value value = HtmlAttribute("value", value)
    let valuef format value = HtmlAttribute("value", Printf.sprintf format value)
    
    let uiEventHandler name handler =
        BlazorEventHandlerAttribute(name, BindMethods.GetEventHandlerValue(Action handler))

    let typedUIEventHandler<'T when 'T :> UIEventArgs> name handler =
        BlazorEventHandlerAttribute(name, BindMethods.GetEventHandlerValue(Action<'T> handler))

    // List of all events here:
    // https://github.com/aspnet/Blazor/blob/dev/src/Microsoft.AspNetCore.Blazor.Browser.JS/src/Rendering/EventForDotNet.ts

    // TODO: may need to provide additional context to make this work correctly, e.g. from which attribute comes the value?
    let onchange handler = typedUIEventHandler<UIChangeEventArgs> "onchange" handler

    let oncopy handler = typedUIEventHandler<UIClipboardEventArgs> "oncopy" handler
    let oncut handler = typedUIEventHandler<UIClipboardEventArgs> "oncut" handler
    let onpaste handler = typedUIEventHandler<UIClipboardEventArgs> "onpaste" handler

    let ondrag handler = typedUIEventHandler<UIDragEventArgs> "ondrag" handler
    let ondragend handler = typedUIEventHandler<UIDragEventArgs> "ondragend" handler
    let ondragenter handler = typedUIEventHandler<UIDragEventArgs> "ondragenter" handler
    let ondragleave handler = typedUIEventHandler<UIDragEventArgs> "ondragleave" handler
    let ondragover handler = typedUIEventHandler<UIDragEventArgs> "ondragover" handler
    let ondragstart handler = typedUIEventHandler<UIDragEventArgs> "ondragstart" handler
    let ondrop handler = typedUIEventHandler<UIDragEventArgs> "ondrop" handler

    let onerror handler = typedUIEventHandler<UIProgressEventArgs> "onerror" handler

    let onfocus handler = typedUIEventHandler<UIFocusEventArgs> "onfocus" handler
    let onblur handler = typedUIEventHandler<UIFocusEventArgs> "onblur" handler
    let onfocusin handler = typedUIEventHandler<UIFocusEventArgs> "onfocusin" handler
    let onfocusout handler = typedUIEventHandler<UIFocusEventArgs> "onfocusout" handler
    
    let onkeydown handler = typedUIEventHandler<UIKeyboardEventArgs> "onkeydown" handler
    let onkeyup handler = typedUIEventHandler<UIKeyboardEventArgs> "onkeyup" handler
    let onkeypress handler = typedUIEventHandler<UIKeyboardEventArgs> "onkeypress" handler

    let oncontextmenu handler = typedUIEventHandler<UIMouseEventArgs> "oncontextmenu" handler
    let onclick handler = typedUIEventHandler<UIMouseEventArgs> "onclick" handler
    let onmouseover handler = typedUIEventHandler<UIMouseEventArgs> "onmouseover" handler
    let onmouseout handler = typedUIEventHandler<UIMouseEventArgs> "onmouseout" handler
    let onmousemove handler = typedUIEventHandler<UIMouseEventArgs> "onmousemove" handler
    let onmousedown handler = typedUIEventHandler<UIMouseEventArgs> "onmousedown" handler
    let onmouseup handler = typedUIEventHandler<UIMouseEventArgs> "onmouseup" handler
    let ondblclick handler = typedUIEventHandler<UIMouseEventArgs> "ondblclick" handler

    let onprogress handler = typedUIEventHandler<UIProgressEventArgs> "onprogress" handler

    let ontouchcancel handler = typedUIEventHandler<UITouchEventArgs> "ontouchcancel" handler
    let ontouchend handler = typedUIEventHandler<UITouchEventArgs> "ontouchend" handler
    let ontouchmove handler = typedUIEventHandler<UITouchEventArgs> "ontouchmove" handler
    let ontouchstart handler = typedUIEventHandler<UITouchEventArgs> "ontouchstart" handler

    let ongotpointercapture handler = typedUIEventHandler<UIPointerEventArgs> "ongotpointercapture" handler
    let onlostpointercapture handler = typedUIEventHandler<UIPointerEventArgs> "onlostpointercapture" handler
    let onpointercancel handler = typedUIEventHandler<UIPointerEventArgs> "onpointercancel" handler
    let onpointerdown handler = typedUIEventHandler<UIPointerEventArgs> "onpointerdown" handler
    let onpointerenter handler = typedUIEventHandler<UIPointerEventArgs> "onpointerenter" handler
    let onpointerleave handler = typedUIEventHandler<UIPointerEventArgs> "onpointerleave" handler
    let onpointermove handler = typedUIEventHandler<UIPointerEventArgs> "onpointermove" handler
    let onpointerout handler = typedUIEventHandler<UIPointerEventArgs> "onpointerout" handler
    let onpointerover handler = typedUIEventHandler<UIPointerEventArgs> "onpointerover" handler
    let onpointerup handler = typedUIEventHandler<UIPointerEventArgs> "onpointerup" handler

    let onmousewheel handler = typedUIEventHandler<UIWheelEventArgs> "onmousewheel" handler

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
        | AddBlazorFrameAttribute of sequence:int * frame:RenderTree.RenderTreeFrame ref
        | AddBlazorObjAttribute of sequence:int * name:string * value:obj
        | AddBlazorEventHandlerAttribute of sequence:int * name:string * handler:MulticastDelegate
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
                        | HtmlConditionalAttribute(name, value) ->
                            yield AddBlazorObjAttribute(step, name, value)
                        | BlazorEventHandlerAttribute(name, handler) ->
                            yield AddBlazorEventHandlerAttribute(step, name, handler)
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
                        | HtmlConditionalAttribute(name, value) ->
                            yield AddBlazorObjAttribute(step, name, value)
                        | BlazorEventHandlerAttribute(name, handler) ->
                            yield AddBlazorEventHandlerAttribute(step, name, handler)
                        | BlazorFrameAttribute(frame) ->
                            yield AddBlazorFrameAttribute(step, frame)
                        | BlazorObjAttribute(name, value) ->
                            yield AddBlazorObjAttribute(step, name, value)
                        step <- step + 1
                ]
            match children with
            | [] -> closeComponent next (fun (sequence', rest) -> cont(sequence', instructions @ rest)) step
            | _ ->
                // TODO: Correctly nest creating the fragment attribute and closing the component within the continuation.
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
            | AddBlazorEventHandlerAttribute(sequence, name, handler) ->
                builder.AddAttribute(sequence, name, handler)
            | AddHtmlAttribute(sequence, name, value) ->
                builder.AddAttribute(sequence, name, value)

[<AbstractClass>]
type Component () =
    inherit BlazorComponent()
    
    abstract Render : unit -> Dom.Node

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        this.Render()
        |> RenderTree.build
        |> RenderTree.render builder

[<AbstractClass>]
type LayoutComponent () =
    inherit BlazorLayoutComponent()
    
    abstract Render : unit -> Dom.Node

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        this.Render()
        |> RenderTree.build
        |> RenderTree.render builder
