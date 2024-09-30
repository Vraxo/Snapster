using Raylib_cs;

namespace Snapster;

public partial class MainScene : Node
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
        images = Directory.GetFiles(@"D:\Parsa Stuff\Screenshots\New folder (2)");
        imageDisplayer = GetNode<TexturedRectangle>("ImageDisplayer/TexturedRectangle");
        base.Start();
    }

    public override void Update()
    {
        HandleArrowKeyInput();
        UpdateKeyHeldState();
        HandleFullscreenToggle();
        base.Update();
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
        // Toggle fullscreen when the space key is pressed
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            Raylib.ToggleFullscreen();
            isFullscreen = !isFullscreen;

            // When fullscreen is activated, update the window size to the screen resolution
            if (isFullscreen)
            {
                int screenWidth = Raylib.GetScreenWidth();   // Fullscreen width
                int screenHeight = Raylib.GetScreenHeight(); // Fullscreen height
                Raylib.SetWindowSize(screenWidth, screenHeight);
            }
            else
            {
                // Optionally, you can reset the window to a specific size when exiting fullscreen
                Raylib.SetWindowSize(1280, 720); // Example resolution for windowed mode
            }
        }
    }

    private void ChangeImage()
    {
        string image = images[index];
        TextureLoader.Instance.Add(images[index], images[index]);
        imageDisplayer.LoadTexture(image);
    }
}