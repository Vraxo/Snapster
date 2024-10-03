using Nodica;
using Raylib_cs;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;

namespace Snapster;

public class MainScene : Node
{
    private string[] images;
    private int index = 0;
    private TexturedRectangle imageDisplayer;
    private float keyHoldTime = 0.0f;
    private const float delayBeforeRepeat = 0.5f;
    private const float repeatRate = 0.1f;
    private bool isRightKeyHeld = false;
    private bool isLeftKeyHeld = false;
    private bool isFullscreen = false;

    public override void Start()
    {
        imageDisplayer = GetNode<TexturedRectangle>("ImageDisplayer/TexturedRectangle");

        if (App.Instance.Args.Length == 0)
        {
            //Environment.Exit(0);
        }
        else
        {
            LoadImageAndDirectory();
        }

        base.Start();
    }

    public override void Update()
    {
        HandleArrowKeyInput();
        UpdateKeyHeldState();
        HandleFullscreenToggle();
        base.Update();
    }

    private void LoadImageAndDirectory()
    {
        string imagePath = App.Instance.Args.First();
        string imageDirectory = Path.GetDirectoryName(imagePath);
        string pngPath = Path.Combine("Resources", Path.GetFileNameWithoutExtension(imagePath) + ".png");

        if (!imagePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        {
            ConvertToPng(imagePath, pngPath);
            imagePath = pngPath;
        }

        TextureLoader.Instance.Add(App.Instance.Args.First(), imagePath);
        imageDisplayer.LoadTexture(App.Instance.Args.First());

        images = Directory.GetFiles(imageDirectory, "*.*")
                          .Where(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                         file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                         file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                          .ToArray();

        index = Array.IndexOf(images, App.Instance.Args.First());

        if (index == -1)
        {
            index = 0;
        }

        if (File.Exists(pngPath))
        {
            File.Delete(pngPath);
        }
    }

    private void HandleArrowKeyInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Right))
        {
            if (index < images.Length - 1)
            {
                index++;
                ChangeImage();
            }
            isRightKeyHeld = true;
            keyHoldTime = 0.0f;
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Left))
        {
            if (index > 0)
            {
                index--;
                ChangeImage();
            }
            isLeftKeyHeld = true;
            keyHoldTime = 0.0f;
        }

        if (Raylib.IsKeyReleased(KeyboardKey.Right))
        {
            isRightKeyHeld = false;
        }

        if (Raylib.IsKeyReleased(KeyboardKey.Left))
        {
            isLeftKeyHeld = false;
        }
    }

    private void UpdateKeyHeldState()
    {
        if (isRightKeyHeld || isLeftKeyHeld)
        {
            keyHoldTime += Raylib.GetFrameTime();

            if (keyHoldTime >= delayBeforeRepeat)
            {
                if (keyHoldTime % repeatRate < Raylib.GetFrameTime())
                {
                    if (isRightKeyHeld && index < images.Length - 1)
                    {
                        index++;
                        ChangeImage();
                    }

                    if (isLeftKeyHeld && index > 0)
                    {
                        index--;
                        ChangeImage();
                    }
                }
            }
        }
    }

    private void HandleFullscreenToggle()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (!Raylib.IsWindowFullscreen())
            {
                Window.PreviousSize = Window.Size;
                int monitor = Raylib.GetCurrentMonitor();
                Raylib.SetWindowSize(Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
                Raylib.ToggleFullscreen();
            }
            else
            {
                Raylib.ToggleFullscreen();
                Raylib.SetWindowSize((int)Window.PreviousSize.X, (int)Window.PreviousSize.Y);
            }
        }
    }

    private void ChangeImage()
    {
        string image = images[index];
        string pngPath = Path.Combine("Resources", Path.GetFileNameWithoutExtension(image) + ".png");

        if (!TextureLoader.Instance.Contains(images[index]))
        {
            if (!image.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                ConvertToPng(image, pngPath);
                image = pngPath;
            }
        }

        TextureLoader.Instance.Add(images[index], image);
        imageDisplayer.LoadTexture(images[index]);

        if (File.Exists(pngPath))
        {
            File.Delete(pngPath);
        }
    }

    private static void ConvertToPng(string inputPath, string outputPath)
    {
        using Image image = Image.Load(inputPath);
        image.SaveAsPng(outputPath);
    }
}