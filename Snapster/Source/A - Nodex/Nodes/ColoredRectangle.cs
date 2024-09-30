using Raylib_cs;

namespace Snapster;

public class ColoredRectangle : ClickableRectangle
{
    public Color FillColor = ThemeLoader.Instance.Colors["Background"];
    public Color OutlineColor = ThemeLoader.Instance.Colors["DefaultOutline"];

    public Action<ColoredRectangle> OnUpdate = (rectangle) => { };

    public ColoredRectangle()
    {
        Size = new(32, 32);
        OriginPreset = OriginPreset.TopLeft;
    }

    public override void Update()
    {
        OnUpdate(this);
        Draw();
    }

    private void Draw()
    {
        Raylib.DrawRectangleV(
            GlobalPosition - Origin, 
            Size, 
            FillColor);

        Raylib.DrawRectangleLines(
            (int)(GlobalPosition.X - Origin.X),
            (int)(GlobalPosition.Y - Origin.Y),
            (int)Size.X,
            (int)Size.Y,
            OutlineColor);
    }
}