﻿namespace CsPacman.Game;

public class Level
{
    public const int Width = 28;
    public const int Height = 32;

    public readonly Point[] teleport =
    [
        new Point(0, 14),
        new Point(-1, 14),
        new Point(-2, 14),
        new Point(-2, -2),
        new Point(Width + 1, -2),
        new Point(Width + 1, 14),
        new Point(Width, 14),
        new Point(Width - 1, 14),
    ];

    public readonly byte[] data = Reload(new byte[Width * Height]);

    public void CopyFrom(Level level)
    {
        Array.Copy(level.data, data, data.Length);
    }

    public bool IsWall(Point p)
    {
        return GetCell(p) >= 2;
    }

    public ref byte GetCell(Point p)
    {
        return ref GetCell(p.X, p.Y);
    }

    public ref byte GetCell(int x, int y)
    {
        return ref x < 0 || y < 0 || x >= Width || y >= Height ? ref data[0] : ref data[x + (y * Width)];
    }

    public void ReloadIfEmpty()
    {
        foreach (byte cell in data)
        {
            if (cell == 1)
            {
                return;
            }
        }
        _ = Reload(data);
    }

    private static byte[] Reload(byte[] result)
    {
        int idx = 0;
        foreach (char ch in File.ReadAllText("Assets/level.txt"))
        {
            switch (ch)
            {
                case ' ':
                    result[idx++] = 0;
                    break;
                case '.':
                    result[idx++] = 1;
                    break;
                case '#':
                    result[idx++] = 2;
                    break;
            }
        }
        return result;
    }
}
