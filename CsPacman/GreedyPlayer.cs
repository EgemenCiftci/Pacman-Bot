using CsPacman.Game;

namespace CsPacman;

public class GreedyPlayer : IPlayer
{
    private static (Point prev, Point current) _hunterGhost;
    private static (Point prev, Point current) _ambushGhost;
    private static (Point prev, Point current) _guardGhost;
    private static (Point prev, Point current) _strangerGhost;
    private static Point? _nextWaypoint;
    private static readonly Dictionary<Point, Point[]> _waypointDict = new()
    {
        { new Point(1, 1), [new Point(6, 1), new Point(1, 5)] },
        { new Point(6, 1), [new Point(1, 1), new Point(12, 1), new Point(6, 5)] },
        { new Point(12, 1), [new Point(6, 1), new Point(12, 5)] },
        { new Point(15, 1), [new Point(21, 1), new Point(15, 5)] },
        { new Point(21, 1), [new Point(15, 1), new Point(26, 1), new Point(21, 5)] },
        { new Point(26, 1), [new Point(21, 1), new Point(26, 5)] },
        { new Point(1, 5), [new Point(6, 5), new Point(1, 1), new Point(1, 8)] },
        { new Point(6, 5), [new Point(1, 5), new Point(9, 5), new Point(6, 1), new Point(6, 8)] },
        { new Point(9, 5), [new Point(6, 5), new Point(12, 5), new Point(9, 8)] },
        { new Point(12, 5), [new Point(9, 5), new Point(15, 5), new Point(12, 1)] },
        { new Point(15, 5), [new Point(12, 5), new Point(18, 5), new Point(15, 1)] },
        { new Point(18, 5), [new Point(15, 5), new Point(21, 5), new Point(18, 8)] },
        { new Point(21, 5), [new Point(18, 5), new Point(26, 5), new Point(21, 1), new Point(21, 8)] },
        { new Point(26, 5), [new Point(21, 5), new Point(26, 1), new Point(26, 8)] },
        { new Point(1, 8), [new Point(1, 5), new Point(6, 8)] },
        { new Point(6, 8), [new Point(1, 8), new Point(6, 5), new Point(6, 14)] },
        { new Point(9, 8), [new Point(12, 8), new Point(9, 5)] },
        { new Point(12, 8), [new Point(9, 8), new Point(12, 11)] },
        { new Point(15, 8), [new Point(18, 8), new Point(15, 11)] },
        { new Point(18, 8), [new Point(15, 8), new Point(18, 5)] },
        { new Point(21, 8), [new Point(26, 8), new Point(21, 5), new Point(21, 14)] },
        { new Point(26, 8), [new Point(21, 8), new Point(26, 5)] },
        { new Point(9, 11), [new Point(12, 11), new Point(9, 14)] },
        { new Point(12, 11), [new Point(9, 11), new Point(15, 11), new Point(12, 8)] },
        { new Point(15, 11), [new Point(12, 11), new Point(18, 11), new Point(15, 8)] },
        { new Point(18, 11), [new Point(15, 11), new Point(18, 14)] },
        { new Point(6, 14), [new Point(21, 14), new Point(6, 8), new Point(6, 20), new Point(9, 14)] },
        { new Point(9, 14), [new Point(6, 14), new Point(9, 11), new Point(9, 17)] },
        { new Point(18, 14), [new Point(21, 14), new Point(18, 11), new Point(18, 17)] },
        { new Point(21, 14), [new Point(18, 14), new Point(6, 14), new Point(21, 8), new Point(21, 20)] },
        { new Point(9, 17), [new Point(18, 17), new Point(9, 14), new Point(9, 20)] },
        { new Point(18, 17), [new Point(9, 17), new Point(18, 14), new Point(18, 20)] },
        { new Point(1, 20), [new Point(6, 20), new Point(1, 23)] },
        { new Point(6, 20), [new Point(1, 20), new Point(9, 20), new Point(6, 23), new Point(6, 14)] },
        { new Point(9, 20), [new Point(6, 20), new Point(12, 20), new Point(9, 17)] },
        { new Point(12, 20), [new Point(9, 20), new Point(12, 23)] },
        { new Point(15, 20), [new Point(18, 20), new Point(15, 23)] },
        { new Point(18, 20), [new Point(15, 20), new Point(21, 20), new Point(18, 17)] },
        { new Point(21, 20), [new Point(18, 20), new Point(26, 20), new Point(21, 23), new Point(21, 14)] },
        { new Point(26, 20), [new Point(21, 20), new Point(26, 23)] },
        { new Point(1, 23), [new Point(1, 20), new Point(3, 23)] },
        { new Point(3, 23), [new Point(1, 23), new Point(3, 26)] },
        { new Point(6, 23), [new Point(6, 20), new Point(6, 26), new Point(9, 23)] },
        { new Point(9, 23), [new Point(6, 23), new Point(12, 23), new Point(9, 26)] },
        { new Point(12, 23), [new Point(9, 23), new Point(15, 23), new Point(12, 20)] },
        { new Point(15, 23), [new Point(12, 23), new Point(18, 23), new Point(15, 20)] },
        { new Point(18, 23), [new Point(15, 23), new Point(21, 23), new Point(18, 26)] },
        { new Point(21, 23), [new Point(18, 23), new Point(21, 20), new Point(21, 26)] },
        { new Point(24, 23), [new Point(26, 23), new Point(24, 26)] },
        { new Point(26, 23), [new Point(24, 23), new Point(26, 20)] },
        { new Point(1, 26), [new Point(3, 26), new Point(1, 29)] },
        { new Point(3, 26), [new Point(1, 26), new Point(6, 26), new Point(3, 23)] },
        { new Point(6, 26), [new Point(3, 26), new Point(6, 23)] },
        { new Point(9, 26), [new Point(12, 26), new Point(9, 23)] },
        { new Point(12, 26), [new Point(9, 26), new Point(12, 29)] },
        { new Point(15, 26), [new Point(18, 26), new Point(15, 29)] },
        { new Point(18, 26), [new Point(15, 26), new Point(18, 23)] },
        { new Point(21, 26), [new Point(24, 26), new Point(21, 23)] },
        { new Point(24, 26), [new Point(21, 26), new Point(26, 26), new Point(24, 23)] },
        { new Point(26, 26), [new Point(24, 26), new Point(26, 29)] },
        { new Point(1, 29), [new Point(12, 29), new Point(1, 26)] },
        { new Point(12, 29), [new Point(1, 29), new Point(15, 29), new Point(12, 26)] },
        { new Point(15, 29), [new Point(12, 29), new Point(26, 29), new Point(15, 26)] },
        { new Point(26, 29), [new Point(15, 29), new Point(26, 26)] }
    };
    private Point ambush;

    public Point Step(StateSnapshot state)
    {
        UpdateGhostPositions(state);

        if (_nextWaypoint == null ||
            (state.player == _nextWaypoint) ||
            (state.player.X == 0 && state.player.Y == 14 && _nextWaypoint?.X == 27 && _nextWaypoint?.Y == 14) ||
            (state.player.X == 27 && state.player.Y == 14 && _nextWaypoint?.X == 0 && _nextWaypoint?.Y == 14))
        {
            _nextWaypoint = GetNextWaypoint(state);
        }

        return GetMove(state.player, _nextWaypoint.GetValueOrDefault());
    }

    private Point GetNextWaypoint(StateSnapshot state)
    {
        var orderedByScore = GetNearestWaypoints(state).Select(x => new
        {
            Waypoint = x,
            Score = CalculateScoreV2(state, (state.player, state.player), x, _hunterGhost, _ambushGhost, _guardGhost, _strangerGhost, state.level.data, 1)
        }).OrderByDescending(x => x.Score);

        return orderedByScore.First().Waypoint;
    }

    private static void UpdateGhostPositions(StateSnapshot state)
    {
        _hunterGhost.prev = _hunterGhost.current == Point.Empty ? state.ghosts[0] : _hunterGhost.current;
        _hunterGhost.current = state.ghosts[0];
        _ambushGhost.prev = _ambushGhost.current == Point.Empty ? state.ghosts[1] : _ambushGhost.current;
        _ambushGhost.current = state.ghosts[1];
        _guardGhost.prev = _guardGhost.current == Point.Empty ? state.ghosts[2] : _guardGhost.current;
        _guardGhost.current = state.ghosts[2];
        _strangerGhost.prev = _strangerGhost.current == Point.Empty ? state.ghosts[3] : _strangerGhost.current;
        _strangerGhost.current = state.ghosts[3];
    }

    private static int CalculateScore(StateSnapshot state, Point player, Point waypoint, (Point prev, Point current) hunterGhost, (Point prev, Point current) ambushGhost, (Point prev, Point current) guardGhost, (Point prev, Point current) strangerGhost, byte[] data, int depth = 1)
    {
        List<(Point prev, Point current)> guardGhosts = [_guardGhost];
        List<(Point prev, Point current)> strangerGhosts = [_strangerGhost];
        byte[] dataCopy = new byte[data.Length];
        Array.Copy(data, dataCopy, dataCopy.Length);
        int collectedPellets = 0;

        while (true)
        {
            int dist = GetManhattanDistance(player, hunterGhost.current);

            if (dist < 1)
            {
                return int.MinValue + dist;
            }

            dist = GetManhattanDistance(player, ambushGhost.current);

            if (dist < 1)
            {
                return int.MinValue + dist;
            }

            if (guardGhosts.Any(x => GetManhattanDistance(player, x.current) < 1) ||
               strangerGhosts.Any(x => GetManhattanDistance(player, x.current) < 1))
            {
                return int.MinValue / 2;
            }

            Point move = GetMove(player, waypoint);
            player.X += move.X;
            player.Y += move.Y;

            if (player.Y == 14)
            {
                if (player.X == -1)
                {
                    player.X = 27;
                }
                else if (player.X == 28)
                {
                    player.X = 0;
                }
            }

            int index = player.X + (player.Y * Level.Width);
            if (dataCopy[index] == 1)
            {
                dataCopy[index] = 0;
                collectedPellets++;
            }

            dist = GetManhattanDistance(player, hunterGhost.current);

            if (dist < 1)
            {
                return int.MinValue + dist;
            }

            dist = GetManhattanDistance(player, ambushGhost.current);

            if (dist < 1)
            {
                return int.MinValue + dist;
            }

            if (guardGhosts.Any(x => GetManhattanDistance(player, x.current) < 1) ||
                strangerGhosts.Any(x => GetManhattanDistance(player, x.current) < 1))
            {
                return int.MinValue / 2;
            }

            if (player == waypoint)
            {
                break;
            }

            Point current = hunterGhost.current;
            hunterGhost.current = GetNextHunterGhostPosition(state, hunterGhost.prev, hunterGhost.current, player);
            hunterGhost.prev = current;

            current = ambushGhost.current;
            ambushGhost.current = GetNextAmbushGhostPosition(state, ambushGhost.prev, ambushGhost.current, player, hunterGhost.prev);
            ambushGhost.prev = current;

            List<(Point prev, Point current)> temps = [];

            foreach ((Point prev, Point current) item in guardGhosts)
            {
                Point[] currents = GetNextGuardGhostPositions(state, item.prev, item.current, dataCopy);

                foreach (Point current0 in currents)
                {
                    temps.Add((item.current, current0));
                }
            }
            guardGhosts = temps;

            temps = [];

            foreach ((Point prev, Point current) item in strangerGhosts)
            {
                Point[] currents = GetNextStrangerGhostPositions(state, item.prev, item.current);

                foreach (Point current0 in currents)
                {
                    temps.Add((item.current, current0));
                }
            }
            strangerGhosts = temps;
        }

        int passiveDistance = 0;
        int activeDistance = 0;
        bool isHunterGhostReached = false;
        bool isAmbushGhostReached = false;
        bool isGuardGhostReached = false;
        bool isStrangerGhostReached = false;
        guardGhost = guardGhosts.Select(x => new { pc = x, distance = GetManhattanDistance(x.current, player) }).OrderBy(x => x.distance).First().pc;
        strangerGhost = strangerGhosts.Select(x => new { pc = x, distance = GetManhattanDistance(x.current, player) }).OrderBy(x => x.distance).First().pc;
        int i = 0;

        while (i < 100 && (!isHunterGhostReached || !isAmbushGhostReached || !isGuardGhostReached || !isStrangerGhostReached))
        {
            Point current;

            if (!isHunterGhostReached)
            {
                current = hunterGhost.current;
                hunterGhost.current = GetNextHunterGhostPosition(state, hunterGhost.prev, hunterGhost.current, player);
                hunterGhost.prev = current;

                activeDistance++;

                if (player == hunterGhost.current)
                {
                    isHunterGhostReached = true;
                    break;
                }
            }

            if (!isAmbushGhostReached)
            {
                current = ambushGhost.current;
                ambushGhost.current = GetNextAmbushGhostPosition(state, ambushGhost.prev, ambushGhost.current, player, hunterGhost.prev);
                ambushGhost.prev = current;

                activeDistance++;

                if (player == ambushGhost.current)
                {
                    isAmbushGhostReached = true;
                    break;
                }
            }

            List<(Point prev, Point current)> temps;

            if (!isGuardGhostReached)
            {
                temps = [];

                foreach ((Point prev, Point current) item in guardGhosts)
                {
                    Point[] currents = GetNextGuardGhostPositions(state, item.prev, item.current, dataCopy);

                    foreach (Point current0 in currents)
                    {
                        temps.Add((item.current, current0));
                    }
                }
                guardGhosts = temps;

                passiveDistance++;

                if (guardGhosts.Any(x => x.current == player))
                {
                    isGuardGhostReached = true;
                    break;
                }
            }

            if (!isStrangerGhostReached)
            {
                temps = [];

                foreach ((Point prev, Point current) item in strangerGhosts)
                {
                    Point[] currents = GetNextStrangerGhostPositions(state, item.prev, item.current);

                    foreach (Point current0 in currents)
                    {
                        temps.Add((item.current, current0));
                    }
                }
                strangerGhosts = temps;

                passiveDistance++;

                if (strangerGhosts.Any(x => x.current == player))
                {
                    isStrangerGhostReached = true;
                    break;
                }
            }

            i++;
        }

        int subScoresSum = 0;

        if (depth > 1)
        {
            depth--;
            List<int> subScores = _waypointDict[waypoint].Select(x => CalculateScore(state, player, x, hunterGhost, ambushGhost, guardGhost, strangerGhost, dataCopy, depth)).ToList();

            if (subScores.Any(x => x == int.MinValue))
            {
                return int.MinValue;
            }

            if (subScores.Any(x => x == int.MinValue / 2))
            {
                return int.MinValue / 2;
            }

            subScoresSum += subScores.Sum();
        }

        return (passiveDistance * 1) + (activeDistance * 2) + (subScoresSum * 3) + (collectedPellets * 100);
    }

    private static int CalculateScoreV2(StateSnapshot state, (Point prev, Point current) player, Point waypoint, (Point prev, Point current) hunterGhost, (Point prev, Point current) ambushGhost, (Point prev, Point current) guardGhost, (Point prev, Point current) strangerGhost, byte[] data, int depth = 1)
    {
        List<(Point prev, Point current)> guardGhosts = [_guardGhost];
        List<(Point prev, Point current)> strangerGhosts = [_strangerGhost];
        byte[] dataCopy = new byte[data.Length];
        Array.Copy(data, dataCopy, dataCopy.Length);
        int collectedPellets = 0;

        while (true)
        {
            Point move = GetMove(player.current, waypoint);
            player.prev = player.current;
            player.current.X += move.X;
            player.current.Y += move.Y;
            if (player.current.Y == 14)
            {
                if (player.current.X == -1)
                {
                    player.current.X = 27;
                }
                else if (player.current.X == 28)
                {
                    player.current.X = 0;
                }
            }

            if (player.current == hunterGhost.current)
            {
                return int.MinValue;
            }

            if (player.current == ambushGhost.current)
            {
                return int.MinValue;
            }

            if (guardGhosts.Any(x => player.current == x.current) ||
                strangerGhosts.Any(x => player.current == x.current))
            {
                return int.MinValue / 2;
            }

            Point current = hunterGhost.current;
            hunterGhost.current = GetNextHunterGhostPosition(state, hunterGhost.prev, hunterGhost.current, player.current);
            hunterGhost.prev = current;

            current = ambushGhost.current;
            ambushGhost.current = GetNextAmbushGhostPosition(state, ambushGhost.prev, ambushGhost.current, player.current, hunterGhost.prev);
            ambushGhost.prev = current;

            List<(Point prev, Point current)> temps = [];
            foreach ((Point prev, Point current) item in guardGhosts)
            {
                Point[] currents = GetNextGuardGhostPositions(state, item.prev, item.current, dataCopy);
                foreach (Point current0 in currents)
                {
                    temps.Add((item.current, current0));
                }
            }
            guardGhosts = temps;

            temps = [];
            foreach ((Point prev, Point current) item in strangerGhosts)
            {
                Point[] currents = GetNextStrangerGhostPositions(state, item.prev, item.current);
                foreach (Point current0 in currents)
                {
                    temps.Add((item.current, current0));
                }
            }
            strangerGhosts = temps;

            if (player.current == hunterGhost.current)
            {
                return int.MinValue;
            }

            if (player.current == ambushGhost.current)
            {
                return int.MinValue;
            }

            if (guardGhosts.Any(x => player.current == x.current) ||
                strangerGhosts.Any(x => player.current == x.current))
            {
                return int.MinValue / 2;
            }

            int index = player.current.X + (player.current.Y * Level.Width);
            if (dataCopy[index] == 1)
            {
                dataCopy[index] = 0;
                collectedPellets++;
            }

            if (player.current == waypoint)
            {
                break;
            }
        }

        int passiveDistance = 0;
        int activeDistance = 0;
        guardGhost = guardGhosts.Select(x => new { pc = x, distance = GetManhattanDistance(x.current, player.current) }).OrderBy(x => x.distance).First().pc;
        strangerGhost = strangerGhosts.Select(x => new { pc = x, distance = GetManhattanDistance(x.current, player.current) }).OrderBy(x => x.distance).First().pc;
        int i = 0;

        while (i < 100)
        {
            Point current;
            current = hunterGhost.current;
            hunterGhost.current = GetNextHunterGhostPosition(state, hunterGhost.prev, hunterGhost.current, player.current);
            hunterGhost.prev = current;
            activeDistance++;
            if (player.current == hunterGhost.current)
            {
                break;
            }

            current = ambushGhost.current;
            ambushGhost.current = GetNextAmbushGhostPosition(state, ambushGhost.prev, ambushGhost.current, player.current, hunterGhost.prev);
            ambushGhost.prev = current;
            activeDistance++;
            if (player.current == ambushGhost.current)
            {
                break;
            }

            List<(Point prev, Point current)> temps;
            temps = [];
            foreach ((Point prev, Point current) item in guardGhosts)
            {
                Point[] currents = GetNextGuardGhostPositions(state, item.prev, item.current, dataCopy);

                foreach (Point current0 in currents)
                {
                    temps.Add((item.current, current0));
                }
            }
            guardGhosts = temps;
            passiveDistance++;
            if (guardGhosts.Any(x => x.current == player.current))
            {
                break;
            }

            temps = [];
            foreach ((Point prev, Point current) item in strangerGhosts)
            {
                Point[] currents = GetNextStrangerGhostPositions(state, item.prev, item.current);

                foreach (Point current0 in currents)
                {
                    temps.Add((item.current, current0));
                }
            }
            strangerGhosts = temps;
            passiveDistance++;
            if (strangerGhosts.Any(x => x.current == player.current))
            {
                break;
            }

            i++;
        }

        int subScoresSum = 0;
        if (depth > 1)
        {
            depth--;
            List<int> subScores = _waypointDict[waypoint].Select(x => CalculateScoreV2(state, player, x, hunterGhost, ambushGhost, guardGhost, strangerGhost, dataCopy, depth)).ToList();
            if (subScores.All(x => x < 0))
            {
                return int.MinValue;
            }
            subScoresSum += subScores.Where(x => x > 0).Sum();
        }

        return (passiveDistance * 1) + (activeDistance * 2) + (subScoresSum * 1) + (collectedPellets * 1000);
    }

    private Point[] GetNearestWaypoints(StateSnapshot state)
    {
        if (_waypointDict.TryGetValue(state.player, out Point[]? value))
        {
            List<Point> list = [.. value];

            if (value.Length > 2)
            {
                // If the player is on a junction, add its position to possibility of waiting there
                list.Add(state.player);
            }

            return [.. list];
        }

        List<Point> waypoints = [];

        foreach (Point move in Moves.All)
        {
            Point neighbor = new(state.player.X + move.X, state.player.Y + move.Y);

            while (true)
            {
                if (state.level.IsWall(neighbor))
                {
                    break;
                }

                if (neighbor.Y == 14)
                {
                    if (neighbor.X == -1)
                    {
                        neighbor.X = 27;
                    }
                    else if (neighbor.X == 28)
                    {
                        neighbor.X = 0;
                    }
                }

                if (_waypointDict.ContainsKey(neighbor))
                {
                    waypoints.Add(neighbor);
                    break;
                }

                neighbor = new(neighbor.X + move.X, neighbor.Y + move.Y);
            }
        }

        return [.. waypoints];
    }

    private static Point GetMove(Point player, Point targetWaypoint)
    {
        // Teleport related moves
        if (player.Y == 14 && targetWaypoint.Y == 14)
        {
            if (player.X >= 18 && targetWaypoint.X == 6)
            {
                return new Point(1, 0);
            }
            else if (player.X <= 9 && targetWaypoint.X == 21)
            {
                return new Point(-1, 0);
            }
        }

        return new Point(Math.Sign(targetWaypoint.X - player.X), Math.Sign(targetWaypoint.Y - player.Y));
    }

    private static int GetManhattanDistance(Point p0, Point p1)
    {
        // Teleport from left
        int dist0 = Math.Abs(p0.X - 0) + Math.Abs(p0.Y - 14);
        int dist1 = Math.Abs(p1.X - 27) + Math.Abs(p1.Y - 14);
        int a = dist0 + dist1;

        // Teleport from right
        dist0 = Math.Abs(p0.X - 27) + Math.Abs(p0.Y - 14);
        dist1 = Math.Abs(p1.X - 0) + Math.Abs(p1.Y - 14);
        int b = dist0 + dist1;

        // Direct
        int c = Math.Abs(p0.X - p1.X) + Math.Abs(p0.Y - p1.Y);

        return Math.Min(a, Math.Min(b, c));
    }

    private static Point GetNextHunterGhostPosition(StateSnapshot state, Point prev, Point current, Point player)
    {
        return Pathfind.Next(state, prev, current, player);
    }

    private static Point GetNextAmbushGhostPosition(StateSnapshot state, Point prev, Point current, Point player, Point hunterGhost)
    {
        Point target = new((player.X * 2) - hunterGhost.X, (player.Y * 2) - hunterGhost.Y);
        return Pathfind.Next(state, prev, current, target);
    }

    private static Point[] GetNextGuardGhostPositions(StateSnapshot state, Point prev, Point current, byte[] data)
    {
        HashSet<Point> points = [];

        for (int sx = 0; sx < Level.Width; sx++)
        {
            for (int sy = 0; sy < Level.Height; sy++)
            {
                int count = 1;

                for (int i = 0; i < Level.Height; i++)
                {
                    for (int j = 0; j < Level.Width; j++)
                    {
                        if (data[j + (i * Level.Width)] == 1)
                        {
                            sx += j;
                            sy += i;
                            count++;
                        }
                    }
                }

                Point target = new(sx / count, sy / count);

                _ = points.Add(Pathfind.Next(state, prev, current, target));
            }
        }

        return [.. points];
    }

    private static Point[] GetNextStrangerGhostPositions(StateSnapshot state, Point prev, Point current)
    {
        HashSet<Point> points = [];

        for (int sx = 0; sx < Level.Width; sx++)
        {
            for (int sy = 0; sy < Level.Height; sy++)
            {
                Point target = new(sx, sy);
                _ = points.Add(Pathfind.Next(state, prev, current, target));
            }
        }

        return [.. points];
    }
}
