using Raylib_cs;
using System.Reflection;

namespace Snapster;

public class App
{
    private static App? instance;

    public Node RootNode;
    public string[] Args;

    private WindowData windowData;

    public static App Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    public void Setup(WindowData windowData, string[] args)
    {
        this.windowData = windowData;
        Args = args;
    }

    public void Run()
    {
        RootNode.Build();
        RootNode.Start();

        RunLoop();
    }

    public void Initialize()
    {
        SetCurrentDirectory();

        Screen.OriginalSize = windowData.Resolution;

        int width = (int)windowData.Resolution.X;
        int height = (int)windowData.Resolution.Y;

        SetWindowFlags();

        Raylib.InitWindow(width, height, windowData.Title);
        Raylib.SetWindowMinSize(width, height);
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);
        Raylib.SetWindowIcon(Raylib.LoadImage("Resources/Icon/Icon.png"));
    }

    private static void SetCurrentDirectory()
    {
        string assemblyLocation = Assembly.GetEntryAssembly().Location;
        Environment.CurrentDirectory = Path.GetDirectoryName(assemblyLocation);
    }

    private static void SetWindowFlags()
    {
        Raylib.SetConfigFlags(
            ConfigFlags.VSyncHint | 
            ConfigFlags.Msaa4xHint |
            ConfigFlags.HighDpiWindow |
            ConfigFlags.ResizableWindow |
            ConfigFlags.AlwaysRunWindow);
    }

    private void RunLoop()
    {
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(ThemeLoader.Instance.Colors["Background"]);
            RootNode.Process();
            Raylib.EndDrawing();

            PrintTree();
        }
    }

    private void PrintTree()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            Console.Clear();
            RootNode.PrintChildren();

            //Random random = new();
            //int r = random.Next(1000);
            //Raylib.TakeScreenshot($"Screenshot{r}.png");
        }
    }
}