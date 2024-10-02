using Raylib_cs;
using System.Reflection;

namespace Nodica;

public class App
{
    private static App? instance;

    public Node RootNode;
    public string[] Args;

    public static App Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    public void Initialize(int width, int height, string title, string[] args)
    {
        Args = args;

        SetCurrentDirectory();

        Window.OriginalSize = new(width, height);

        SetWindowFlags();

        Raylib.InitWindow(width, height, title);
        Raylib.SetWindowMinSize(width, height);
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);
        Raylib.SetWindowIcon(Raylib.LoadImage("Resources/Icon/Icon.png"));
    }

    public void Run()
    {
        RootNode.Build();
        RootNode.Start();

        RunLoop();
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