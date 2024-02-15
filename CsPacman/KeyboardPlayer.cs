using CsPacman.Game;

namespace CsPacman;

public class KeyboardPlayer : IPlayer
{
    private readonly List<Keys> pressed = [];
    private Keys last;

    public KeyboardPlayer(Control ui)
    {
        ui.PreviewKeyDown += (s, e) => HandleKey(e.KeyData, true);
        ui.KeyUp += (s, e) => HandleKey(e.KeyData, false);
    }

    public Point Step(StateSnapshot state)
    {
        Point delta = pressed.Count > 0 ? Direction(pressed[^1]) : Direction(last);
        last = Keys.None;
        return delta;
    }

    private void HandleKey(Keys key, bool press)
    {
        switch (key)
        {
            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
                if (press)
                {
                    if (!pressed.Contains(key))
                    {
                        pressed.Add(last = key);
                    }
                }
                else
                {
                    _ = pressed.Remove(key);
                }
                break;
        }
    }

    private static Point Direction(Keys key)
    {
        return key switch
        {
            Keys.Left => Moves.Left,
            Keys.Right => Moves.Right,
            Keys.Up => Moves.Up,
            Keys.Down => Moves.Down,
            _ => new Point(),
        };
    }
}
