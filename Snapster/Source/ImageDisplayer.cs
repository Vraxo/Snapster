namespace Snapster;

public partial class ImageDisplayer : AspectRatioContainer
{
    public override void Start()
    {
        TextureLoader.Instance.Add("DefaultTexture", "Resources/Texture.png");
        GetNode<TexturedRectangle>("TexturedRectangle").LoadTexture("DefaultTexture");
    }

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
