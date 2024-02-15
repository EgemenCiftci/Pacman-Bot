namespace CsPacman.Game;

public class Animation(int rate, params Sprite[] sprites)
{
    private readonly Sprite[] sprites = sprites;
    private readonly int rate = rate;

    public void Draw(Graphics g, Point pixel, int tick)
    {
        Sprite sprite = sprites[tick / rate % sprites.Length];
        Rectangle rect = new(pixel.X, pixel.Y, sprite.rect.Width, sprite.rect.Height);
        g.DrawImage(sprite.image, rect, sprite.rect, GraphicsUnit.Pixel);
    }
}
