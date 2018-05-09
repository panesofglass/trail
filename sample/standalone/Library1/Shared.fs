namespace BlazorApp1.Shared

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Net.Http
open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Layouts
open Microsoft.AspNetCore.Blazor.Routing
open Trail

type NavMenu () =
    inherit Trail.Component()

    override __.Render() =
        Dom.div [Dom.HtmlAttribute("class", "main-nav")] [
            Dom.div [Dom.HtmlAttribute("class", "navbar navbar-inverse")] [
                Dom.div [Dom.HtmlAttribute("class", "navbar-header")] [
                    Dom.button [
                            Dom.HtmlAttribute("type", "button")
                            Dom.HtmlAttribute("class", "navbar-toggle")
                            Dom.HtmlAttribute("data-toggle", "collapse")
                            Dom.HtmlAttribute("data-target", ".navbar-collapse")
                        ] [
                            Dom.span [Dom.HtmlAttribute("class", "sr-only")] [
                                Dom.text "Toggle navigation"
                            ]
                            Dom.span [Dom.HtmlAttribute("class", "icon-bar")] []
                            Dom.span [Dom.HtmlAttribute("class", "icon-bar")] []
                            Dom.span [Dom.HtmlAttribute("class", "icon-bar")] []
                        ]
                    Dom.a [Dom.HtmlAttribute("class", "navbar-brand"); Dom.HtmlAttribute("href", "/")] [
                        Dom.text "BlazorApp1"
                    ]
                ]
                Dom.div [Dom.HtmlAttribute("class", "clearfix")] []
                Dom.div [Dom.HtmlAttribute("class", "navbar-collapse collapse")] [
                    Dom.ul [Dom.HtmlAttribute("class", "nav navbar-nav")] [
                        Dom.li [] [
                            Dom.comp<NavLink> [
                                    Dom.HtmlAttribute("href", "/")
                                    Dom.BlazorObjAttribute("Match", NavLinkMatch.All)
                                ] [
                                    Dom.span [Dom.HtmlAttribute("class", "glyphicon glyphicon-home")] []
                                    Dom.text "Home"
                                ]
                        ]
                        Dom.li [] [
                            Dom.comp<NavLink> [
                                    Dom.HtmlAttribute("href", "/counter")
                                    Dom.BlazorObjAttribute("Match", NavLinkMatch.All)
                                ] [
                                    Dom.span [Dom.HtmlAttribute("class", "glyphicon glyphicon-home")] []
                                    Dom.text "Counter"
                                ]
                        ]
                        Dom.li [] [
                            Dom.comp<NavLink> [
                                    Dom.HtmlAttribute("href", "/fetchdata")
                                    Dom.BlazorObjAttribute("Match", NavLinkMatch.All)
                                ] [
                                    Dom.span [Dom.HtmlAttribute("class", "glyphicon glyphicon-home")] []
                                    Dom.text "Fetch data"
                                ]
                        ]
                    ]
                ]
            ]
        ]

type MainLayout () =
    inherit Trail.LayoutComponent()

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
    [<Parameter>]
    member val Title : string = Unchecked.defaultof<string> with get, set
