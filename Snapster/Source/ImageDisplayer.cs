using Nodica;

namespace Snapster;

public class ImageDisplayer : AspectRatioContainer
{
    public override void Update()
    {
        UpdateSize();
        base.Update();
    }

    private void UpdateSize()
    {
        Size = Window.Size;
    }
}