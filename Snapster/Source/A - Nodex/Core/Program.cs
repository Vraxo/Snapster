using Raylib_cs;
using System.Reflection;

namespace Snapster;

public class Program
{
    public Node RootNode;
    public string[] Args;

    private readonly WindowData windowData;

    public Program(WindowData windowData, string[] args)
    {
        this.windowData = windowData;
        Args = args;
    }

    public void Run()
    {
        RootNode.Program = this;
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
        
        //Scene scene = new(rootNode);
        //var mainScene = scene.Instantiate<MainScene>();
        //RootNode = mainScene;

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
                Raylib.ClearBackground(windowData.ClearColor);
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

            //Random random = new();
            //int r = random.Next(1000);

            //Raylib.TakeScreenshot($"Screenshot{r}.png");

            RootNode.PrintChildren();
        }
    }
}