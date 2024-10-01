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
            ClearColor = ThemeLoader.Instance.Colors["Background"]
        };

        //MainScene rootNode = new();

        Program program = new(windowData, args);
        program.Initialize();

        var rootNode = new Scene("MainScene.txt").Instantiate<MainScene>();

        program.RootNode = rootNode;
        program.Run();
    }
}