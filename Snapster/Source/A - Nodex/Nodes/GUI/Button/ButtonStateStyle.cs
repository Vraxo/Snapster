namespace Snapster;

public class ButtonStateStyle
{
    public float FontSpacing { get; set; } = 1;

    public float Roundness { get; set; } = 0;
    public float OutlineThickness { get; set; } = 0;
    public float FontSize { get; set; } = 16;
    public Font Font { get; set; } = FontLoader.Instance.Fonts["RobotoMono 32"];
    public Color TextColor { get; set; } = ThemeLoader.Instance.Colors["Text"];
    public Color FillColor { get; set; } = ThemeLoader.Instance.Colors["DefaultFill"];
    public Color OutlineColor { get; set; } = ThemeLoader.Instance.Colors["DefaultOutline"];
}