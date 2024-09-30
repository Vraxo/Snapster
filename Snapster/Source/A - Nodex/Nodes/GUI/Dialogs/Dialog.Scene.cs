﻿namespace Snapster;

public partial class Dialog : Node2D
{
    public override void Build()
    {
        AddChild(new ColoredRectangle
        {
            Size = new(300, 150),
            InheritOrigin = true
        }, "Background");

        AddChild(new Button
        {
            Text = "X",
            Size = new(25, 25),
            InheritOrigin = true,
            Layer = ClickableLayer.DialogButtons,
            Style = new()
            {
                TextColor = Color.Red
            },
            OnUpdate = (button) =>
            {
                float x = 300 - 35;
                float y = 10;

                button.Position = new(x, y);
            }
        }, "CloseButton");

        AddChild(new Label
        {
            Position = new(10, 50),
            InheritOrigin = true,
        }, "Message");
    }
}