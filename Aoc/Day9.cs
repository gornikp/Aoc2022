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
        public Point(Point a)
        {
            X = a.X;
            Y = a.Y;
        }
        public Point Move(int x, int y) => new Point(X + x, Y + y);
        public Point Move(Point a) => new Point(X + a.X, Y + a.Y);

        public static Point Create(Direction direction)
        {
            switch (direction)
            {
                case Direction.U:
                    return new Point(0, 1);
                case Direction.D:
                    return new Point(0, -1);
                case Direction.L:
                    return new Point(-1, 0);
                case Direction.R:
                    return new Point(1, 0);
                default:
                    return new Point(0, 0);
            }
        }

        public (Point Point, Direction Direction) MoveTail(Point head, Direction direction)
        {
            if (Math.Abs(head.X - X) > 1 && Math.Abs(head.Y - Y) == 0) // horizontal move
            {
                var move = direction == Direction.R || direction == Direction.RU || direction == Direction.RD? 1 : -1; // Any Horizontal move
                return (Move(move, 0), move > 0 ? Direction.R : Direction.L); // Return current move
            }
            if (Math.Abs(head.X - X) == 0 && Math.Abs(head.Y - Y) > 1) // vertical move
            {
                var move = direction == Direction.U || direction == Direction.LU || direction == Direction.RU? 1 : -1; // Any vertical move
                return (Move(0, move), move > 0 ? Direction.U : Direction.D); // Return current move
            }
            
            //diagonal move
            if (Math.Abs(head.X - X) + Math.Abs(head.Y - Y) > 2)
            {
                return direction switch
                {
                    Direction.U => (new Point(head.X, Y + 1), head.X > X ? Direction.RU : Direction.LU),
                    Direction.D => (new Point(head.X, Y - 1), head.X > X ? Direction.RD : Direction.LD),
                    Direction.L => (new Point(X - 1, head.Y), head.Y - Y > 0 ? Direction.LU : Direction.LD),
                    Direction.R => (new Point(X + 1, head.Y), head.Y - Y > 0 ? Direction.RU : Direction.RD),
                    
                    Direction.LU => (Move(-1,1), Direction.LU),
                    Direction.LD => (Move(-1,-1), Direction.LD),
                    Direction.RD => (Move(1,-1), Direction.RD),
                    Direction.RU => (Move(1,1), Direction.RU),
                    _ => (this, Direction.NoMove)
                };
            }

            return (this, Direction.NoMove);
        }
    }

    protected override void Part1()
    {
        List<Point> tailPoints = new List<Point>();
        var currentHeadPoint = new Point(0, 0);
        var currentTailPoint = new Point(0, 0);
        foreach (var moveString in _data)
        {
            var splitted = moveString.Split(" ", StringSplitOptions.TrimEntries);
            var direction = Enum.Parse<Direction>(splitted[0]);
            var move = Point.Create(direction);
            for (int i = 0; i < int.Parse(splitted[1]); i++)
            {
                currentHeadPoint = currentHeadPoint.Move(move);
                currentTailPoint = currentTailPoint.MoveTail(currentHeadPoint, direction).Point;
                tailPoints.Add(currentTailPoint);
            }
        }

        var distinctPoints = tailPoints.Distinct().ToList();
        Console.WriteLine($"Part1: {distinctPoints.Count}");
    }

    protected override void Part2()
    {
        List<Point> tailPoints = new List<Point>();
        var tail = new List<(Point Point, Direction Direction)>(9);
        for (int i = 0; i < 10; i++)
        {
            tail.Add((new Point(11, 5), Direction.NoMove));
        }
        foreach (var moveString in _data)
        {
            var splitted = moveString.Split(" ", StringSplitOptions.TrimEntries);
            var direction = Enum.Parse<Direction>(splitted[0]);
            var move = Point.Create(direction);
            for (int i = 0; i < int.Parse(splitted[1]); i++)
            {
                tail[0] =  (tail[0].Point.Move(move), direction);
                
                for (int j = 1; j < tail.Count; j++)
                {
                    tail[j] = tail[j].Point.MoveTail(tail[j-1].Point, tail[j-1].Direction);
                }
                tailPoints.Add(tail[9].Point);
            }
        }

        var distinctPoints = tailPoints.Distinct().ToList();
        Console.WriteLine($"Part2: {distinctPoints.Count}");
    }

    void PrintSnake(List<(Point Point, Direction Direction)> snake)
    {
        var table = new List<List<string>>();
        for (int i = 0; i < 100; i++)
        {
            var innerLine = new List<string>();
            for (int j = 0; j < 100; j++)
            {
                innerLine.Add(".");
            }
            table.Add(innerLine);
        }

        for (int i = snake.Count - 1; i >= 0 ; i--)
        {
            table[snake[i].Point.Y][snake[i].Point.X] = $"{i}";
            table[snake[0].Point.Y][snake[0].Point.X] = "H";
            table[5][11] = "s";
        }
        
        foreach (var line in table.AsEnumerable().Reverse())
        {
            var xd = string.Concat(line);
            Console.WriteLine(xd);
        }
    }

    public enum Direction
    {
        NoMove = 0,
        U = 1,
        D = 2,
        L = 3,
        R = 4,
        LU = 5,
        LD = 6,
        RD = 7,
        RU = 8
    }
}