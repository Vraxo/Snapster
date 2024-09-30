using Raylib_cs;

namespace Snapster;

public class TextureLoader
{
    public Dictionary<string, Texture2D> Textures = [];

    private static TextureLoader? instance;

    public static TextureLoader Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    public void Add(string name, Texture2D texture)
    {
        Textures.Add(name, texture);
    }

    public void Remove(string name)
    {
        Textures.Remove(name);
    }
}