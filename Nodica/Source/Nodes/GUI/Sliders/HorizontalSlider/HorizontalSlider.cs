using Raylib_cs;

namespace Nodica;

public partial class HorizontalSlider : BaseSlider
{
    public HorizontalSlider()
    {
        Size = new(100, 10);
        OriginPreset = OriginPreset.CenterLeft;
    }

    protected override void UpdatePercentage()
    {
        if (Raylib.IsWindowMinimized())
        {
            return;
        }

        float currentPosition = Grabber.GlobalPosition.X;
        float minPos = GlobalPosition.X;
        float maxPos = minPos + Size.X;

        Percentage = Math.Clamp((currentPosition - minPos) / (maxPos - minPos), 0, 1);
    }

    public override void MoveGrabber(int direction)
    {
        if (MaxExternalValue == 0)
        {
            return;
        }

        float unit = Size.X / MaxExternalValue;
        float x = Grabber.GlobalPosition.X + direction * unit;
        float y = Grabber.GlobalPosition.Y;

        Grabber.GlobalPosition = new(x, y);

        UpdatePercentageBasedOnGrabber();
    }

    //public override void MoveGrabber(float)
    //{
    //    if (MaxExternalValue == 0)
    //    {
    //        return;
    //    }
    //
    //    float unit = Size.X / MaxExternalValue;
    //    float x = Grabber.GlobalPosition.X + direction * unit;
    //    float y = Grabber.GlobalPosition.Y;
    //
    //    Grabber.GlobalPosition = new(x, y);
    //
    //    UpdatePercentageBasedOnGrabber();
    //}

    protected override void MoveGrabberToPercentage(float percentage)
    {
        float x = GlobalPosition.X + percentage * Size.X;
        float y = GlobalPosition.Y;

        Grabber.GlobalPosition = new(x, y);
    }

    protected override void HandleClicks()
    {
        if (IsMouseOver())
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && OnTopLeft)
            {
                float x = Raylib.GetMousePosition().X;
                float y = Grabber.GlobalPosition.Y;

                Grabber.GlobalPosition = new(x, y);
                Grabber.Pressed = true;
            }

            if (Raylib.IsMouseButtonPressed(MouseButton.Right) && OnTopRight)
            {
                if (ResetOnRitghtClick)
                {
                    RevertToDefaultPercentage();
                    OnPercentageChanged();
                    OnReleased();
                }
            }
        }
    }

    protected override void Draw()
    {
        if (!Visible)
        {
            return;
        }

        Vector2 position = GlobalPosition - Origin;

        Rectangle emptyRectangle = new()
        {
            Position = position,
            Size = Size
        };

        DrawOutline(emptyRectangle, EmptyStyle.Current);

        Raylib.DrawRectangleRounded(
            emptyRectangle,
            EmptyStyle.Current.Roundness,
            (int)Size.Y,
            EmptyStyle.Current.FillColor);

        if (Percentage == 0)
        {
            return;
        }

        Rectangle filledRectangle = new()
        {
            Position = position,
            Size = new(Percentage * Size.X, Size.Y)
        };

        DrawOutline(filledRectangle, FilledStyle.Current);

        Raylib.DrawRectangleRounded(
            filledRectangle,
            FilledStyle.Current.Roundness,
            (int)Size.Y,
            FilledStyle.Current.FillColor);
    }

    private void DrawOutline(Rectangle rectangle, ButtonStateStyle style)
    {
        if (style.OutlineThickness <= 0)
        {
            return;
        }

        for (int i = 0; i <= style.OutlineThickness; i++)
        {
            Rectangle outlineRectangle = new()
            {
                Position = rectangle.Position - new Vector2(i, i),
                Size = new(rectangle.Size.X + i + 1, rectangle.Size.Y + i + 1)
            };

            Raylib.DrawRectangleRounded(
                outlineRectangle,
                style.Roundness,
                (int)rectangle.Size.Y,
                style.OutlineColor);
        }
    }
}