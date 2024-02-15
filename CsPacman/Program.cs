using CsPacman.Game;
using Timer = System.Windows.Forms.Timer;

namespace CsPacman;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        GameUI ui = new();
        GreedyPlayer player = new();
        //KeyboardPlayer player = new(ui);
        GameLoop game = new(0, player);

        Timer timer = new()
        {
            Interval = 1000 / 30
        };
        timer.Tick += (s, e) =>
        {
            if (!game.Step(ui))
            {
                timer.Stop();
            }
        };
        timer.Start();

        Application.Run(new Form
        {
            Text = "C# Pacman Challenge",
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            StartPosition = FormStartPosition.CenterScreen,
            MaximizeBox = false,
            Controls = { ui }
        });
    }
}