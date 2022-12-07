using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aoc;

public class Day2 : BaseDay
{
    List<(string Theirs, string Ours)> _games = new();

    protected override void ReadFile()
    {
        using var stream = File.OpenRead("Day2.txt");
        using var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine().Split(' ');
            _games.Add((line[0], line[1]));
        }
    }

    protected override void Part1()
    {
        var totalScore = 0;
        foreach (var game in _games)
        {
            totalScore += Score(game);
        }

        Console.WriteLine($"TTotal Score using the strategy is: {totalScore}");
    }
    
    protected override void Part2()
    {
        const string RockB = "X"; // 1 point
        const string PaperB = "Y"; // 2 points
        const string ScissorsB = "Z"; // 3 points
        var totalScore = 0;
        foreach (var game in _games)
        {
            totalScore += Score((game.Theirs, GetMove(game)));
        }

        Console.WriteLine($"Total Score for 2nd part is: {totalScore}");
    }

    private string GetMove((string Theirs, string Ours) line)
    {
        const string RockA = "A";
        const string PaperA = "B";
        const string ScissorsA = "C";
        const string Loose = "X";
        const string Draw = "Y"; 
        const string Win = "Z"; 
        const string RockB = "X"; // 1 point
        const string PaperB = "Y"; // 2 points
        const string ScissorsB = "Z"; // 3 points

        if (line.Theirs == RockA)
        {
            if (line.Ours == Loose) return ScissorsB;
            if (line.Ours == Draw) return RockB;
            if (line.Ours == Win) return PaperB;
        }
        else if (line.Theirs == PaperA)
        {
            if (line.Ours == Loose) return RockB;
            if (line.Ours == Draw) return PaperB;
            if (line.Ours == Win) return ScissorsB;
        }
        else if (line.Theirs == ScissorsA)
        {
            if (line.Ours == Loose) return PaperB;
            if (line.Ours == Draw) return ScissorsB;
            if (line.Ours == Win) return RockB;
        }

        return "Invalid";
    }

    private int Score((string Theirs, string Ours) line)
    {
        var roundScore = MoveScore(line.Ours);
        roundScore += FightScore(line);
        return roundScore;
    }
    
    private int FightScore((string Theirs, string Ours) line)
    {
        if ((line.Ours == "X" && line.Theirs == "C") || (line.Ours == "Y" && line.Theirs == "A") || (line.Ours == "Z" && line.Theirs == "B")) return 6;
        if ((line.Ours == "X" && line.Theirs == "A") || (line.Ours == "Y" && line.Theirs == "B") || (line.Ours == "Z" && line.Theirs == "C")) return 3;
        return 0;
    }

    private int MoveScore(string ours)
    {
        if (ours == "X") return 1;
        else if (ours == "Y") return 2;
        return 3;
    }
}