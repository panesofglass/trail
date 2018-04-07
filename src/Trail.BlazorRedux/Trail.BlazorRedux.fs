namespace Trail

[<AbstractClass>]
type ReduxComponent<'TModel, 'TAction> () =
    inherit BlazorRedux.ReduxComponent<'TModel, 'TAction>()

    abstract Render : unit -> Dom.Node

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        this.Render()
        |> RenderTree.build
        |> RenderTree.render builder
