namespace Nodica;

public partial class Dialog : Node2D
{
    public override void Start()
    {
        Origin = GetNode<ColoredRectangle>("Background").Size / 2;
        GetNode<ClickManager>("/root/ClickManager").MinLayer = ClickableLayer.DialogButtons;
        GetNode<Button>("CloseButton").LeftClicked += OnCloseButtonLeftClicked;
    }

    public override void Update()
    {
        UpdatePosition();
    }

    protected void Close()
    {
        GetNode<ClickManager>("/root/ClickManager").MinLayer = 0;
        Destroy();
    }

    private void OnCloseButtonLeftClicked(object? sender, EventArgs e)
    {
        Close();
    }

    private void UpdatePosition()
    {
        Position = Window.Size / 2;
    }
}