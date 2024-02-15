namespace CsPacman.Game;

public class StateSnapshot(int ghosts)
{
    public readonly Level level = new();
    public readonly Point[] ghosts = new Point[ghosts];
    public Point player;
    public int score;
    public int tick;
}
