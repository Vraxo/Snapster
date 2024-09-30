using Raylib_cs;

namespace Snapster;

public partial class BaseSlider
{
    public abstract class BaseGrabber : ClickableRectangle
    {
        public ButtonStyle Style = new();
        public bool Pressed = false;
        public Action<BaseGrabber> OnUpdate = (button) => { };

        protected bool alreadyClicked = false;
        protected bool initialPositionSet = false;

        public BaseGrabber()
        {
            Size = new(18, 18);
            InheritPosition = false;
        }

        public override void Start()
        {
            UpdatePosition(true);
            base.Start();
        }

        public override void Update()
        {
            OnUpdate(this);
            UpdatePosition();
            CheckForClicks();
            Draw();
            base.Update();
        }

        protected abstract void UpdatePosition(bool initial = false);

        private void CheckForClicks()
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                if (!IsMouseOver())
                {
                    alreadyClicked = true;
                }
            }

            if (IsMouseOver())
            {
                Style.Current = Style.Hover;

                if (Raylib.IsMouseButtonDown(MouseButton.Left) && !alreadyClicked && OnTopLeft)
                {
                    OnTopLeft = false;
                    Pressed = true;
                    alreadyClicked = true;
                }

                if (Pressed)
                {
                    Style.Current = Style.Pressed;
                }
            }
            else
            {
                Style.Current = Style.Default;
            }

            if (Pressed)
            {
                Style.Current = Style.Pressed;
            }

            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                Pressed = false;
                Style.Current = Style.Default;
                alreadyClicked = false;
            }
        }

        private void Draw()
        {
            if (!(Visible && ReadyForVisibility))
            {
                return;
            }

            DrawShape();
        }

        private void DrawShape()
        {
            DrawOutline();
            DrawInside();
        }

        private void DrawInside()
        {
            Rectangle rectangle = new()
            {
                Position = GlobalPosition - Origin,
                Size = Size
            };

            Raylib.DrawRectangleRounded(
                rectangle,
                Style.Current.Roundness,
                (int)Size.Y,
                Style.Current.FillColor);
        }

        private void DrawOutline()
        {
            if (Style.Current.OutlineThickness <= 0)
            {
                return;
            }

            Vector2 position = GlobalPosition - Origin;

            Rectangle rectangle = new()
            {
                Position = position,
                Size = Size
            };

            for (int i = 0; i <= Style.Current.OutlineThickness; i++)
            {
                Rectangle outlineRectangle = new()
                {
                    Position = rectangle.Position - new Vector2(i, i),
                    Size = new(rectangle.Size.X + i + 1, rectangle.Size.Y + i + 1)
                };

                Raylib.DrawRectangleRounded(
                    outlineRectangle,
                    Style.Current.Roundness,
                    (int)rectangle.Size.X,
                    Style.Current.OutlineColor);
            }
        }

        //private void Draw()
        //{
        //    if (!(Visible && ReadyForVisibility))
        //    {
        //        return;
        //    }
        //
        //    float x = (float)Math.Round(GlobalPosition.X);
        //    float y = (float)Math.Round(GlobalPosition.Y);
        //
        //    Vector2 temporaryPosition = new(x, y);
        //
        //    DrawShapeOutline(temporaryPosition);
        //    DrawShapeInside(temporaryPosition);
        //}
        //
        //private void DrawShapeInside(Vector2 position)
        //{
        //    Rectangle rectangle = new()
        //    {
        //        Position = position - Origin,
        //        Size = Size
        //    };
        //
        //    Raylib.DrawRectangleRounded(
        //        rectangle,
        //        Style.Current.Roundness,
        //        (int)Size.Y,
        //        Style.Current.FillColor);
        //}
        //
        //private void DrawShapeOutline(Vector2 position)
        //{
        //    if (Style.Current.OutlineThickness < 0)
        //    {
        //        return;
        //    }
        //
        //    for (int i = 0; i <= Style.Current.OutlineThickness; i++)
        //    {
        //        Rectangle rectangle = new()
        //        {
        //            Position = position - Origin - new Vector2(i, i),
        //            Size = new(Size.X + i + 1, Size.Y + i + 1)
        //        };
        //
        //        Raylib.DrawRectangleRounded(
        //            rectangle,
        //            Style.Current.Roundness,
        //            (int)Size.Y,
        //            Style.Current.OutlineColor);
        //    }
        //}
    }
}