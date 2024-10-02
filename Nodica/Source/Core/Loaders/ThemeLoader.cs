using System.Text.Json;

namespace Nodica;

public class ThemeLoader
{
    public Dictionary<string, Color> Colors = new()
    {
        { "Background", new(16, 16, 16, 255) },
        { "DefaultFill", new(42, 42, 42, 255) },
        { "DefaultOutline", new(61, 61, 61, 255) },
        { "HoverFill", new(50, 50, 50, 255) },
        { "HoverOutline", new(71, 71, 71, 255) },
        { "Accent", new(71, 114, 179, 255) },
        { "AccentLighter", new(91, 134, 199, 255) },
        { "AccentDarker", new(51, 94, 159, 235) },
        { "PressedOutline", new(61, 61, 61, 255) },
        { "TextBoxPressedFill", new(68, 68, 68, 255) },
        { "SliderEmptyFill", new(101, 101, 101, 255) },
        { "SliderFillColor", new(71, 114, 179, 255) },
        { "Text", new(255, 255, 255, 255) }
    };

    private static ThemeLoader? instance;

    public static ThemeLoader Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    private ThemeLoader()
    {
        //Save();
        string name = File.ReadAllText("Resources/Themes/Theme.txt");
        //LoadTexture(name);
    }

    private void Save()
    {
        var serializableColors = Colors.ToDictionary(
            kvp => kvp.Key,
            kvp => new
            {
                kvp.Value.R,
                kvp.Value.G,
                kvp.Value.B,
                kvp.Value.A
            }
        );

        var json = JsonSerializer.Serialize(serializableColors, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText("Resources/Themes/Dark.json", json);
    }

    public void Load(string fileName)
    {
        string json = File.ReadAllText($"Resources/Themes/{fileName}.json");
        var deserializedColors = JsonSerializer.Deserialize<Dictionary<string, JsonColor>>(json);

        Colors.Clear();

        foreach (var kvp in deserializedColors)
        {
            Colors[kvp.Key] = new(kvp.Value.R, kvp.Value.G, kvp.Value.B, kvp.Value.A);
        }
    }

    private class JsonColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
    }
}