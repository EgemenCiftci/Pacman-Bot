using CsPacman.Game;

namespace CsPacman.Characters;

public class StrangerGhost(StateSnapshot snapshot, Random random, Point position) : Character(snapshot, position, 6)
{
    private readonly Random random = random;
    private Point target;
    private int timeout;

    public override Animation GetAnimation()
    {
        return Animations.Ghost2[animation];
    }

    protected override Point NextTarget(Point prev, Point current)
    {
        if (--timeout <= 0 || current == target)
        {
            target = new Point(random.Next(Level.Width), random.Next(Level.Height));
            timeout = Math.Abs(target.X - current.X) + Math.Abs(target.Y - current.Y);
        }
        return Pathfind.Next(snapshot, prev, current, target);
    }
}
