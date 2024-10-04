using Raylib_cs;

namespace Nodica;

public class TexturedRectangle : Node2D
{
    public Texture2D Texture { get; set; } = Raylib.LoadTexture("");
    public Action<TexturedRectangle> OnUpdate = (rectangle) => { };

    public TexturedRectangle()
    {
        Size = new(32, 32);
    }

    public override void Update()
    {
        OnUpdate(this);
        Draw();
        base.Update();
    }

    public void LoadTexture(string name, bool resize = false)
    {
        Texture = TextureLoader.Instance.Textures[name];

        if (resize)
        {
            Size = new(Texture.Width, Texture.Height);
            //Size = new(1280/4, 720/4);
        }
    }

    private void Draw()
    {
        if (!(Visible && ReadyForVisibility))
        {
            return;
        }

        Rectangle source = new(0, 0, Texture.Width, Texture.Height);
        Rectangle destination = new(GlobalPosition, Size);

        Raylib.DrawTexturePro(
            Texture,
            source,
            destination,
            Origin,
            0,
            Color.White);
    }
}