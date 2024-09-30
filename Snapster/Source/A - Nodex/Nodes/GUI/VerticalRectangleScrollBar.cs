//using Raylib_cs;
//
//namespace Snapster;
//
//class VerticalRectangleScrollBar : Node2D
//{
//    public Action<VerticalRectangleScrollBar> OnUpdate = (slider) => { };
//    public Button Grabber;
//    public Button TopButton;
//    public Button BottomButton;
//
//    private float _value = 0;
//
//    public float Value
//    {
//        get => _value;
//
//        set
//        {
//            _value = value;
//            _value = Math.Clamp(_value, 0, 1);
//        }
//    }
//
//    public override void Ready()
//    {
//        AddTopButton();
//        AddBottomButton();
//        AddMiddleButton();
//    }
//
//    public override void Update()
//    {
//        Value = Grabber.Position.Y / (Size.Y - Grabber.Size.Y);
//        BottomButton.Position.Y = Size.Y + 8;
//
//        DrawRectangle();
//        SnapMiddleButtonToClickedPosition();
//
//        OnUpdate(this);
//
//        base.Update();
//    }
//
//    private void OnTopButtonLeftClicked(object? sender, EventArgs e)
//    {
//        Grabber.Position.Y -= Grabber.Size.Y;
//        Value -= BottomButton.Size.Y / Size.Y;
//    }
//
//    private void DrawRectangle()
//    {
//        Rectangle rectangle = new()
//        {
//            Position = GlobalPosition - Origin,
//            Size = Size
//        };
//        
//        Raylib.DrawRectangleRounded(rectangle, 0.0F, (int)Size.X, FillColor.DarkGray);
//    }
//
//    private void AddTopButton()
//    {
//        TopButton = new()
//        {
//            Position = new(0, -8),
//            Size = new(Size.X, 10),
//        };
//
//        TopButton.LeftClicked += OnTopButtonLeftClicked;
//
//        AddChild(TopButton);
//    }
//
//    private void AddBottomButton()
//    {
//        BottomButton = new()
//        {
//            Position = new(0, Size.Y),
//            Size = new(Size.X, 10),
//        };
//
//        BottomButton.LeftClicked += OnBottomButtonLeftClicked;
//
//        AddChild(BottomButton);
//    }
//
//    private void OnBottomButtonLeftClicked(object? sender, EventArgs e)
//    {
//        Grabber.Position.Y += Grabber.Size.Y;
//        Value += BottomButton.Size.Y / Size.Y;
//    }
//
//    private void AddMiddleButton()
//    {
//        Grabber = new()
//        {
//            Size = new(Size.X, Size.X),
//            Origin = new(Size.X / 2, 0),
//            OriginPreset = OriginPreset.TopCenter,
//            OnUpdate = (Button) =>
//            {
//                if (Button.PressedLeft)
//                {
//                    Button.Position.Y = Raylib.GetMousePosition().Y - Button.Size.Y;
//                }
//
//                float maxY = Size.Y - Button.Size.Y;
//
//                if (Button.Position.Y > maxY)
//                {
//                    Button.Position.Y = maxY;
//                }
//
//                if (Button.Position.Y < 0)
//                {
//                    Button.Position.Y = 0;
//                }
//            }
//        };
//
//        AddChild(Grabber);
//
//        Grabber.FilledStyle.Default.FillColor = new(128, 128, 128, 255);
//    }
//
//    private void SnapMiddleButtonToClickedPosition()
//    {
//        if (IsMouseOver() && Raylib.IsMouseButtonPressed(MouseButton.Left))
//        {
//            Grabber.Position.Y = Raylib.GetMousePosition().Y - Grabber.Size.Y;
//        }
//    }
//
//    private bool IsMouseOver()
//    {
//        Vector2 mousePosition = Raylib.GetMousePosition();
//
//        bool matchX1 = mousePosition.X > GlobalPosition.X - Origin.X;
//        bool matchX2 = mousePosition.X < GlobalPosition.X + Size.X - Origin.X;
//
//        bool matchY1 = mousePosition.Y > GlobalPosition.Y - Origin.Y;
//        bool matchY2 = mousePosition.Y < GlobalPosition.Y + Size.Y - Origin.Y;
//
//        return matchX1 && matchX2 && matchY1 && matchY2;
//    }
//}