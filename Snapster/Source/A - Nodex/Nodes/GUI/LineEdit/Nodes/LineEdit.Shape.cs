using Raylib_cs;

namespace Snapster;

public partial class LineEdit
{
    private class Shape : Node2D
    {
        private LineEdit parent;

        public override void Start()
        {
            parent = GetParent<LineEdit>();
        }

        public override void Update()
        {
            Draw();
            base.Update();
        }

        private void Draw()
        {
            if (!(parent.Visible && parent.ReadyForVisibility))
            {
                return;
            }

            DrawOutline();
            DrawInside();
        }

        private void DrawOutline()
        {
            if (parent.Style.Current.OutlineThickness <= 0)
            {
                return;
            }

            for (int i = 1; i <= parent.Style.Current.OutlineThickness; i++)
            {
                Vector2 offset = new(i / 2f, i / 2f);

                Rectangle rectangle = new()
                {
                    Position = parent.GlobalPosition - parent.Origin - offset,
                    Size = new(parent.Size.X + i, parent.Size.Y + i)
                };

                Raylib.DrawRectangleRounded(
                    rectangle,
                    parent.Style.Current.Roundness,
                    (int)parent.Size.Y,
                    parent.Style.Current.OutlineColor);
            }
        }

        private void DrawInside()
        {
            Rectangle rectangle = new()
            {
                Position = parent.GlobalPosition - parent.Origin,
                Size = parent.Size
            };

            Raylib.DrawRectangleRounded(
                rectangle,
                parent.Style.Current.Roundness,
                (int)parent.Size.Y,
                parent.Style.Current.FillColor);
        }
    }
}