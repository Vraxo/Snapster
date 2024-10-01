namespace Snapster;

public partial class ImageDisplayer : AspectRatioContainer
{
    public override void Update()
    {
        UpdateSize();
        base.Update();
    }

    private void UpdateSize()
    {
        Size = Screen.Size;
    }
}