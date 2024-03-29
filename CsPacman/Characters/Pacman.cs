﻿using CsPacman.Game;

namespace CsPacman.Characters;

public class Pacman(StateSnapshot snapshot, Point position, IPlayer player) : Character(snapshot, position, 6)
{
    private readonly IPlayer player = player;
    private IEnumerator<Point>? teleporting;

    public override Animation GetAnimation()
    {
        return Animations.Pacman[animation];
    }

    protected override Point NextTarget(Point prev, Point current)
    {
        if (teleporting == null)
        {
            if (current == snapshot.level.teleport[0])
            {
                teleporting = ForwardTeleport(snapshot.level.teleport);
            }
            if (current == snapshot.level.teleport[^1])
            {
                teleporting = BackwardTeleport(snapshot.level.teleport);
            }
        }
        if (teleporting != null)
        {
            if (teleporting.MoveNext())
            {
                return teleporting.Current;
            }
            teleporting = null;
        }
        Point delta = player.Step(snapshot);
        Point next = new(current.X + delta.X, current.Y + delta.Y);
        return snapshot.level.IsWall(next) ? current : next;
    }

    private static IEnumerator<Point> ForwardTeleport(Point[] points)
    {
        for (int i = 1; i < points.Length; i++)
        {
            yield return points[i];
        }
    }

    private static IEnumerator<Point> BackwardTeleport(Point[] points)
    {
        for (int i = points.Length - 1; i-- > 0;)
        {
            yield return points[i];
        }
    }
}
