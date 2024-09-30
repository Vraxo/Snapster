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
        Size = Screen.Size;

        //Position = Screen.Size / 2;
        //
        //Vector2 center = Screen.Size / 2;
        //
        //Vector2 textureSize = new Vector2(Texture.Width, Texture.Height);
        //
        //Vector2 totalRatio = Screen.Size / textureSize;
        //float ratio = totalRatio.X < totalRatio.Y ? totalRatio.X : totalRatio.Y;
        //
        //Vector2 newSize = textureSize * ratio;
        //
        //Size = newSize;

        base.Update();
    }
}
