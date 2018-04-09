module Tests

open Trail
open Trail.RenderTree
open Expecto

[<Tests>]
let tests =
  testList "DOM -> AST tests" [
    testCase "Text is valid content" <| fun _ ->
        let dom = Dom.text "Hi, there"
        let ast = RenderTree.build dom
        let expected = [ AddTextContent(0, "Hi, there") ]
        Expect.equal ast expected "Should generate a single AddTextContent expression."

    testCase "Empty div element expected AST" <| fun _ ->
        let dom = Dom.div [] []
        let ast = RenderTree.build dom
        let expected = [
            OpenElement(0, "div")
            CloseElement
            AddTextContent(1, "\n")
        ]
        Expect.equal ast expected "An empty div element should produce a three part AST"

    testCase "a element with href produces expected AST" <| fun _ ->
        let dom = Dom.a [Dom.HtmlAttribute("href","https://github.com/panesofglass/trail")] []
        let ast = RenderTree.build dom
        let expected = [
            OpenElement(0, "a")
            AddHtmlAttribute(1, "href", "https://github.com/panesofglass/trail")
            CloseElement
            AddTextContent(2, "\n")
        ]
        Expect.equal ast expected "An anchor element with an href attribute should produce a four part AST"

    testCase "Nested elements produce expected AST" <| fun _ ->
        let dom =
            Dom.div [] [
                Dom.a [Dom.HtmlAttribute("href","https://github.com/panesofglass/trail")] [Dom.text "link"]
            ]
        let ast = RenderTree.build dom
        let expected = [
            OpenElement(0, "div")
            OpenElement(1, "a")
            AddHtmlAttribute(2, "href", "https://github.com/panesofglass/trail")
            AddTextContent(3, "link")
            CloseElement
            AddTextContent(4, "\n")
            CloseElement
            AddTextContent(5, "\n")
        ]
        Expect.equal ast expected "An anchor element with an href attribute nested within a div should produce a seven part AST"

    testCase "Fragments wrap sibling nodes with no common parent but don't generate a node in the AST" <| fun _ ->
        let dom =
            Dom.Fragment [
                Dom.h1 [] [Dom.text "Navigation"]
                Dom.div [] [
                    Dom.a [Dom.HtmlAttribute("href","https://github.com/panesofglass/trail")] [Dom.text "link"]
                ]
            ]
        let ast = RenderTree.build dom
        let expected = [
            OpenElement(0, "h1")
            AddTextContent(1, "Navigation")
            CloseElement
            AddTextContent(2, "\n")
            OpenElement(3, "div")
            OpenElement(4, "a")
            AddHtmlAttribute(5, "href", "https://github.com/panesofglass/trail")
            AddTextContent(6, "link")
            CloseElement
            AddTextContent(7, "\n")
            CloseElement
            AddTextContent(8, "\n")
        ]
        Expect.equal ast expected "A Fragment should not produce its own AST nodes."

    //testCase "universe exists (╭ರᴥ•́)" <| fun _ ->
    //  let subject = true
    //  Expect.isTrue subject "I compute, therefore I am."

    //testCase "when true is not (should fail)" <| fun _ ->
    //  let subject = false
    //  Expect.isTrue subject "I should fail because the subject is false"

    //testCase "I'm skipped (should skip)" <| fun _ ->
    //  Tests.skiptest "Yup, waiting for a sunny day..."

    //testCase "I'm always fail (should fail)" <| fun _ ->
    //  Tests.failtest "This was expected..."

    //testCase "contains things" <| fun _ ->
    //  Expect.containsAll [| 2; 3; 4 |] [| 2; 4 |]
    //                     "This is the case; {2,3,4} contains {2,4}"

    //testCase "contains things (should fail)" <| fun _ ->
    //  Expect.containsAll [| 2; 3; 4 |] [| 2; 4; 1 |]
    //                     "Expecting we have one (1) in there"

    //testCase "Sometimes I want to ༼ノಠل͟ಠ༽ノ ︵ ┻━┻" <| fun _ ->
    //  Expect.equal "abcdëf" "abcdef" "These should equal"

    //test "I am (should fail)" {
    //  "╰〳 ಠ 益 ಠೃ 〵╯" |> Expect.equal true false
    //}
  ]

