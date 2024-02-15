namespace CsPacman.Game;

public class Sprite(Image image, int x, int y)
{
    public readonly Image image = image;
    public readonly Rectangle rect = new(x * 32, y * 32, 32, 32);
}
