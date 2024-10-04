using Raylib_cs;

namespace Nodica;

public class AspectRatioContainer : Node2D
{
    public override void Update()
    {
        Position = Window.Size / 2;
        Size = Window.Size;

        foreach (Node2D child in Children.Cast<Node2D>())
        {
            Vector2 center = Size / 2;

            Vector2 totalRatio = Size / child.Size;
            float ratio = totalRatio.X < totalRatio.Y ? totalRatio.X : totalRatio.Y;

            Vector2 newSize = child.Size * ratio;

            child.Size = newSize;
            //child.Position = center;
        }

        base.Update();
    }
}