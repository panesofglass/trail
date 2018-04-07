namespace Trail

[<AbstractClass>]
type FlatwareComponent<'TMsg, 'TModel> () =
    inherit Flatware.FlatwareComponent<'TMsg, 'TModel>()

    abstract Render : unit -> Dom.Node

    override this.BuildRenderTree(builder) =
        base.BuildRenderTree(builder)
        this.Render()
        |> RenderTree.build
        |> RenderTree.render builder
