namespace Snapster;

public partial class ImageDisplayer : AspectRatioContainer
{
    public override void Build()
    {
        AddChild(new TexturedRectangle());
    }
}
