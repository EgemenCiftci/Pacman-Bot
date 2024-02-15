using CsPacman.Game;

namespace CsPacman.Characters;

public class GuardGhost(StateSnapshot snapshot, Random random, Point position) : Character(snapshot, position, 8)
{
    private readonly Random random = random;

    public override Animation GetAnimation()
    {
        return Animations.Ghost3[animation];
    }

    protected override Point NextTarget(Point prev, Point current)
    {
        int sx = random.Next(Level.Width);
        int sy = random.Next(Level.Height);
        int count = 1;

        for (int i = 0; i < Level.Height; i++)
        {
            for (int j = 0; j < Level.Width; j++)
            {
                if (snapshot.level.GetCell(j, i) == 1)
                {
                    sx += j;
                    sy += i;
                    count++;
                }
            }
        }

        Point target = new(sx / count, sy / count);

        return Pathfind.Next(snapshot, prev, current, target);
    }
}
