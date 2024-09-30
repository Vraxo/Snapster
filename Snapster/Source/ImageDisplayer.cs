using Raylib_cs;
using System.Numerics; // For Vector2

namespace Snapster;

public partial class ImageDisplayer : AspectRatioContainer
{
    public override void Start()
    {
        GetNode<TexturedRectangle>("TexturedRectangle").LoadTexture("Resources/Texture.png");
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
