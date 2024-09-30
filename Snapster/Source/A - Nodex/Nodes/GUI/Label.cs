using Raylib_cs;

namespace Snapster;

public class Label : Node2D
{
    public Color Color { get; set; } = ThemeLoader.Instance.Colors["Text"];
    public uint FontSize { get; set; } = 16;
    public Font Font { get; set; } = FontLoader.Instance.Fonts["RobotoMono 32"];
    public int MaxCharacters = -1;
    public float AvailableWidth { get; set; } = -1;
    public Action<Label> OnUpdate = (label) => { };

    private string _text = "";
    public string Text
    {
        get => _text;

        set
        {
            _text = value;
            displayedText = value;
        }
    }

    private string displayedText = "";

    public Label()
    {
        OriginPreset = OriginPreset.CenterLeft;
    }

    public override void Update()
    {
        OnUpdate(this);
        UpdateSize();
        LimitDisplayedText();
        Draw();
        base.Update();
    }

    private void UpdateSize()
    {
        Size = Raylib.MeasureTextEx(
            Font, 
            Text, 
            FontSize, 
            1);
    }

    private void Draw()
    {
        if (!(Visible && ReadyForVisibility))
        {
            return;
        }

        Raylib.DrawTextEx(
            Font, 
            displayedText, 
            GlobalPosition - Origin, 
            FontSize, 
            1,
            Color);
    }

    private void LimitDisplayedText()
    {
        if (AvailableWidth < 1)
        {
            if (Name == "Message")
            {
                Console.WriteLine("returning");
            }

            return;
        }

        float characterWidth = GetCharacterWidth();
        int numFittingCharacters = (int)(AvailableWidth / characterWidth);

        if (numFittingCharacters <= 0)
        {
            displayedText = "";
        }
        else if (numFittingCharacters < Text.Length)
        {
            string trimmedText = Text[..numFittingCharacters];
            displayedText = ReplaceLastThreeWithDots(trimmedText);
        }
        else
        {
            displayedText = Text;
        }
    }

    private float GetCharacterWidth()
    {
        float width = Raylib.MeasureTextEx(
            Font,
            " ",
            FontSize,
            1).X;

        return width;
    }

    private static string ReplaceLastThreeWithDots(string input)
    {
        if (input.Length > 3)
        {
            string trimmedText = input[..^3];
            return trimmedText + "...";
        }
        else
        {
            return input;
        }
    }
}