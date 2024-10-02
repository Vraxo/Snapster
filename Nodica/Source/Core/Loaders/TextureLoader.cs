using Raylib_cs;

namespace Nodica;

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

    public void Add(string name, string path)
    {
        if (!Textures.ContainsKey(name))
        {
            Textures.Add(name, Raylib.LoadTexture(path));
        }
    }

    public void Remove(string name)
    {
        if (Textures.ContainsKey(name))
        {
            Textures.Remove(name);
        }
    }

    public bool Contains(string name)
    {
        return Textures.ContainsKey(name);
    }
}