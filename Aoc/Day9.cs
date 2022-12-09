using System.Diagnostics.Tracing;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Aoc;

public class Day9 : BaseDay
{
    private List<string> _data = new List<string>();

    protected override void ReadFile()
    {
        using var stream = File.OpenRead("Day9.txt");
        using var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line is null) continue;
            _data.Add(line);
        }
    }

    public record Point(int X, int Y)
    {
        public Point Move(Point a) => new Point(X + a.X, Y + a.Y);
        
        public static Point Create(Direction direction)
        {
            return direction switch
            {
                Direction.U => new Point(0, 1),
                Direction.D => new Point(0, -1),
                Direction.L => new Point(-1, 0),
                Direction.R => new Point(1, 0),
                _ => new Point(0, 0)
            };
        }

        public Point MoveTail(Point head)
        {
            if (Math.Abs(head.X - X) > 1 ||  Math.Abs(head.Y - Y) > 1)
            {
                int x = Math.Sign(head.X - X);
                int y = Math.Sign(head.Y - Y);
                return Move(new Point(x,y));
            }
            return this;
        }
    }

    protected override void Part1()
    {
        var distinctPoints = SimulateRope(2);
        Console.WriteLine($"Part1: {distinctPoints.Count}");
    }

    protected override void Part2()
    {
        var distinctPoints = SimulateRope(10);
        Console.WriteLine($"Part2: {distinctPoints.Count}");
    }

    private List<Point> SimulateRope(int tailLength)
    {
        List<Point> tailPoints = new List<Point>();
        var rope = new List<Point>(tailLength);
        for (int i = 0; i < tailLength; i++)
        {
            rope.Add(new Point(0, 0));
        }

        foreach (var moveString in _data)
        {
            var splitted = moveString.Split(" ", StringSplitOptions.TrimEntries);
            var move = Point.Create(Enum.Parse<Direction>(splitted[0]));
            var numberOfMoves = int.Parse(splitted[1]);
            for (int i = 0; i < numberOfMoves; i++)
            {
                rope[0] = rope[0].Move(move);

                for (int j = 1; j < rope.Count; j++)
                {
                    rope[j] = rope[j].MoveTail(rope[j - 1]);
                }

                tailPoints.Add(rope[^1]);
            }
        }

        var distinctPoints = tailPoints.Distinct().ToList();
        return distinctPoints;
    }

    public enum Direction
    {
        U = 1,
        D = 2,
        L = 3,
        R = 4,
    }
}