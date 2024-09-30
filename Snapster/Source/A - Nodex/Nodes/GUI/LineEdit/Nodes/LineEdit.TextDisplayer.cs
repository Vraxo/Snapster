namespace Snapster;

public partial class LineEdit
{
    private class TextDisplayer : BaseText
    {
        protected override string GetText()
        {
            return parent.Secret ?
                new string(parent.SecretCharacter, parent.Text.Length) :
                parent.Text.Substring(parent.TextStartIndex, Math.Min(parent.Text.Length - parent.TextStartIndex, parent.GetDisplayableCharactersCount()));
        }

        protected override bool ShouldSkipDrawing()
        {
            return false;
        }
    }
}