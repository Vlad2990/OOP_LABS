using System;
using System.Runtime.InteropServices;
using System.Drawing;
public sealed class SettingsManager
{
    private static readonly SettingsManager instance = new SettingsManager();

    private SettingsManager()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
    }

    public static SettingsManager Instance => instance;

    public void SetColor(ConsoleColor background)
    {
        if (background == ConsoleColor.Black)
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Black;
        }
        Console.BackgroundColor = background;
        Console.Clear();
    }
}