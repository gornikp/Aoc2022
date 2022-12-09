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
        public Point Move(int x, int y) => new Point(X + x, Y + y);
        public static Point Create(string direction)
        {
            return direction switch
            {
                "U" => new Point(0, 1),
                "D" => new Point(0, -1),
                "L" => new Point(-1, 0),
                "R" => new Point(1, 0),
                _ => new Point(0, 0)
            };
        }
        
        public Point MoveTail(Point head)
        {
            if (Math.Abs(head.X - X) > 1 ||  Math.Abs(head.Y - Y) > 1) 
                return Move(Math.Sign(head.X - X), Math.Sign(head.Y - Y));
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
        var tailPoints = new List<Point>();
        var rope = Enumerable.Range(0, tailLength).Select(x => new Point(0, 0)).ToList();

        foreach (var moveString in _data)
        {
            var splitted = moveString.Split(" ", StringSplitOptions.TrimEntries);
            var move = Point.Create(splitted[0]);
            var numberOfMoves = int.Parse(splitted[1]);
            
            for (int i = 0; i < numberOfMoves; i++)
            {
                rope[0] = rope[0].Move(move.X, move.Y);

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
}