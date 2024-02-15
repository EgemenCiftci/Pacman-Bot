using CsPacman.Game;

namespace CsPacman.Characters;

public class HunterGhost(StateSnapshot snapshot, Point position) : Character(snapshot, position, 9)
{
    public override Animation GetAnimation()
    {
        return Animations.Ghost1[animation];
    }

    protected override Point NextTarget(Point prev, Point current)
    {
        return Pathfind.Next(snapshot, prev, current, snapshot.player);
    }
}
