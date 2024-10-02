namespace Snapster;

public class EntryPoint
{
    [STAThread]
    public static void Main(string[] args)
    {
        WindowData windowData = new()
        {
            Title = "Snapster",
            Resolution = new(640, 480),
        };

        App.Instance.Setup(windowData, args);
        App.Instance.Initialize();

        var rootNode = new Scene("MainScene.txt").Instantiate<MainScene>();

        App.Instance.RootNode = rootNode;
        App.Instance.Run();
    }
}