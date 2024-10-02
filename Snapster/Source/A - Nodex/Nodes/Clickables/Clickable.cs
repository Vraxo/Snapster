namespace Snapster;

public abstract class Clickable : Node2D
{
    public bool OnTopLeft = false;
    public bool OnTopRight = false;
    //public int Layer = 0;

    protected ClickManager? clickManager;

    public override void Start()
    {
        clickManager = GetNode<ClickManager>("/root/ClickManager");

        if (clickManager == null)
        {
            //App.RootNode.AddChild(new ClickManager());
            RootNode.Children.Insert(0, new ClickManager { Name = "ClickManager" });
            clickManager = GetNode<ClickManager>("/root/ClickManager");
        }

        clickManager.Add(this);
    }

    public override void Destroy()
    {
        //if (Active)
        //{
            clickManager.Remove(this);
        //}

        base.Destroy();
    }

    public abstract bool IsMouseOver();
}