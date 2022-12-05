using System;
using System.Diagnostics;

namespace Advent;

class Program
{
    private static Stopwatch _stopwatch;

    private const int From = 1;
    private const int To = 25;

    private static readonly int[,] Times = new int[25, 2];

    static void Main(string[] args)
    {
        _stopwatch = new Stopwatch();

        if (IncludeDay(11))
        {
            var day11 = new Day11();
            Output(1, 1, "Elf with max", day11.CountMoves(InputData.Day11Arrangement,false));
            Output(1, 2, "Three elves with max", day11.CountMoves(InputData.Day11Arrangement,true));
        }

        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("Day       Part 1    Part 2");
        for (int i = 0; i < 25; i++)
        {
            Console.WriteLine($"{i + 1,-10}{Times[i, 0],5} ms  {Times[i, 1],5} ms");
        }
    }

    static bool IncludeDay(int day)
    {
        if (day < From || day > To) return false;

        _stopwatch.Reset();
        _stopwatch.Start();
        Console.WriteLine();
        Console.WriteLine($"DAY {day}");
        Console.WriteLine($"==========");

        return true;
    }

    static void Output(int day, int part, string name, object answer)
    {
        var time = _stopwatch.ElapsedMilliseconds;
        Times[day - 1, part - 1] = (int) time;
        Console.WriteLine($"{time} ms - {name}: {answer}");
        _stopwatch.Reset();
        _stopwatch.Start();
    }
}
