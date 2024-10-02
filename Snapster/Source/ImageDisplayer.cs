using SixLabors.ImageSharp;
using System;
using Image = SixLabors.ImageSharp.Image;

namespace Snapster;

public class ImageDisplayer : AspectRatioContainer
{
    private string[] images;
    TexturedRectangle texturedRectangle;
    private int index = 0;

    public override void Start()
    {
        if (App.Instance.Args.Length == 0)
        {
            //images = Directory.GetFiles(@"D:\Parsa Stuff\Screenshots\New folder (2)");
            //TextureLoader.Instance.Add("DefaultTexture", "Resources/Texture.png");
            //imageDisplayer.LoadTexture("DefaultTexture");
            //index = 0;
        }
        else
        {
            LoadImageAndDirectory();
        }

        base.Start();
    }

    public override void Update()
    {
        UpdateSize();
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
        texturedRectangle.LoadTexture(App.Instance.Args.First());

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

    private void UpdateSize()
    {
        Size = Screen.Size;
    }

    private static void ConvertToPng(string inputPath, string outputPath)
    {
        using Image image = Image.Load(inputPath);
        image.SaveAsPng(outputPath);
    }
}