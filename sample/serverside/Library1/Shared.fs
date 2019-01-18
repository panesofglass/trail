namespace BlazorApp1.Shared

open Microsoft.AspNetCore.Blazor
open Microsoft.AspNetCore.Blazor.Components
open Microsoft.AspNetCore.Blazor.Routing
open Trail

type NavMenu () =
    inherit Trail.Component()

    let mutable collapseNavMenu = true

    let toggleNavMenu (ev:UIMouseEventArgs) =
        collapseNavMenu <- not collapseNavMenu

    override __.Render() =
        Dom.Fragment [
            Dom.div [Attr.className "top-row pl-4 navbar navbar-dark"] [
                Dom.a [Attr.className "navbar-brand"; Attr.href "/"] [
                    Dom.text "BlazorApp1"
                ]
                Dom.button [
                        Attr.className "navbar-toggler"
                        Attr.onclick toggleNavMenu
                    ] [
                        Dom.span [Attr.className "navbar-toggler-icon"] []
                    ]
            ]

            Dom.div [
                    Attr.className (if collapseNavMenu then "collapse" else null)
                    Attr.onclick toggleNavMenu
                ] [
                Dom.ul [Attr.className "nav flex-column"] [
                    Dom.li [Attr.className "nav-item px-3"] [
                        Dom.comp<NavLink> [
                                Attr.className "nav-link"
                                Attr.href "/"
                                Dom.BlazorObjAttribute("Match", NavLinkMatch.All)
                            ] [
                                Dom.span [Attr.className "oi oi-home"; Dom.HtmlAttribute("aria-hidden", "true")] []
                                Dom.text "Home"
                            ]
                    ]
                    Dom.li [Attr.className "nav-item px-3"] [
                        Dom.comp<NavLink> [
                                Attr.className "nav-link"
                                Attr.href "/counter"
                            ] [
                                Dom.span [Attr.className "oi oi-plus"; Dom.HtmlAttribute("aria-hidden", "true")] []
                                Dom.text "Counter"
                            ]
                    ]
                    Dom.li [Attr.className "nav-item px-3"] [
                        Dom.comp<NavLink> [
                                Attr.className "nav-link"
                                Attr.href "/fetchdata"
                            ] [
                                Dom.span [Attr.className "oi oi-list-rich"; Dom.HtmlAttribute("aria-hidden", "true")] []
                                Dom.text "Fetch data"
                            ]
                    ]
                ]
            ]
        ]

type MainLayout () =
    inherit Trail.LayoutComponent()

    override this.Render() =
        Dom.Fragment [
            Dom.div [Attr.className "sidebar"] [
                Dom.comp<NavMenu> [] []
            ]
            Dom.div [Attr.className "main"] [
                Dom.div [Attr.className "top-row px-4"] [
                    Dom.a [
                            Attr.href "http://blazor.net"
                            Dom.HtmlAttribute("target", "_blank")
                            Attr.className "ml-md-auto"
                        ] [
                            Dom.text "About"
                        ]
                ]
                Dom.div [Attr.className "content px-4"] [
                    Dom.content this.Body
                ]
            ]
        ]

type SurveyPrompt () =
    inherit Trail.Component()

    override this.Render() =
        Dom.div [Dom.HtmlAttribute("class", "alert alert-secondary mt-4"); Dom.HtmlAttribute("role", "alert")] [
            Dom.span [Dom.HtmlAttribute("class", "oi oi-pencil mr-2"); Dom.HtmlAttribute("aria-hidden", "true")] []
            Dom.strong [] [Dom.text this.Title]

            Dom.span [Attr.className "text-nowrap"] [
                Dom.text "Please take our "
                Dom.a [
                        Dom.HtmlAttribute("target", "_blank")
                        Attr.className "font-weight-bold"
                        Attr.href "https://go.microsoft.com/fwlink/?linkid=2006382"
                    ] [
                        Dom.text "brief survey"
                    ]
            ]
            Dom.text " and tell us what you think."
        ]

    // This is to demonstrate how a parent component can supply parameters
    [<Parameter>]
    member val Title : string = Unchecked.defaultof<string> with get, set
