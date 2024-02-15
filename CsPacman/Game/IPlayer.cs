namespace CsPacman.Game;

public static class Moves
{
    public static readonly Point Up = new(0, -1);
    public static readonly Point Down = new(0, 1);
    public static readonly Point Left = new(-1, 0);
    public static readonly Point Right = new(1, 0);

    public static readonly Point[] All = [Up, Down, Left, Right];
}

public interface IPlayer
{
    Point Step(StateSnapshot state);
}
