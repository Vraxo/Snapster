namespace Snapster;

public class AspectRatioContainer : Node2D
{
    public override void Update()
    {
        foreach (Node2D child in Children.Cast<Node2D>())
        {
            Vector2 center = Size / 2;
            
            Vector2 textureSize = child.Size;
            
            Vector2 totalRatio = Screen.Size / textureSize;
            float ratio = totalRatio.X < totalRatio.Y ? totalRatio.X : totalRatio.Y;
            
            Vector2 newSize = textureSize * ratio;
            
            child.Size = newSize;
            child.Position = Size / 2;
        }

        base.Update();
    }
}