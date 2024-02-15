using CsPacman.Game;

namespace CsPacman;

public class RandomPlayer : IPlayer
{
    private readonly Random random = new();

    public Point Step(StateSnapshot state)
    {
        while (true)
        {
            Point move = Moves.All[random.Next(Moves.All.Length)];
            Point target = new(state.player.X + move.X, state.player.Y + move.Y);
            if (!state.level.IsWall(target))
            {
                return move;
            }
        }
    }
}
