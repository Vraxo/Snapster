namespace Snapster;

public partial class LineEdit : ClickableRectangle
{
    public override void Build()
    {
        AddChild(new Shape());

        AddChild(new TextDisplayer());

        AddChild(new PlaceholderTextDisplayer());

        AddChild(new Caret(), "Caret");
    }
}