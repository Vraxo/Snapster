﻿using Nodica;

namespace Snapster;

public class ImageDisplayer : AspectRatioContainer
{
    public override void Update()
    {
        UpdateSize();
        base.Update();
    }

    private void UpdateSize()
    {
        Position = new(0, 0);
        Size = Window.Size;
    }
}