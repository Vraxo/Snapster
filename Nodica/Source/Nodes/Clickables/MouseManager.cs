using Raylib_cs;

namespace Nodica;

public class MouseManager
{
    public static MouseManager Instance => instance ??= new MouseManager();
    private static MouseManager? instance;

    public int MinLayer = int.MinValue;

    private List<Clickable> clickables = [];

    public void Register(Clickable clickable)
    {
        clickables.Add(clickable);
    }

    public void Unregister(Clickable clickable)
    {
        clickables.Remove(clickable);
    }

    public void Update()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            foreach (Clickable clickable in clickables)
            {
                if (clickable.IsMouseOver())
                {
                    if (clickable.Layer > MinLayer)
                    {
                        clickable.OnTopLeft = true;
                    }
                }
            }
        }
    }
}