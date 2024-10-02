namespace Snapster;

public class EntryPoint
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.Instance.Initialize(640, 480, "Snapster", args);

        var rootNode = new Scene("MainScene.txt").Instantiate<MainScene>();

        App.Instance.RootNode = rootNode;
        App.Instance.Run();
    }
}