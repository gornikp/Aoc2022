using System.Reflection;

namespace Aoc;

public class Day3 : BaseDay
{
    List<(string CompartmentA, string CompartmentB)> _compartments = new();

    protected override void ReadFile()
    {
        var elfNo = 1;
        using var stream = File.OpenRead("Day3.txt");
        using var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line == null) continue;
            var compartmentA = line.Substring(0, line.Length/2);
            var compartmentB = line.Substring(line.Length/2);
            _compartments.Add((compartmentA, compartmentB));
        }
    }

    protected override void Part1()
    {
        var sum = _compartments.Sum(x => GetCommon(x).Sum(x => char.IsUpper(x) ? x - 38 : x - 96 ));
        Console.WriteLine($"All common products sum: {sum}");
    }
    
    static List<char> GetCommon((string CompartmentA, string CompartmentB) xd)
    {
        return xd.CompartmentB.Where(xd.CompartmentA.Contains).Distinct().ToList();
    }
    
    protected override void Part2()
    {
        var chuj = new List<String> { "Śliwa", "Serocki", "Ącki", "Adamksi", "Ekulaw", "Ęcki", "Górnik" };
        var chuj2 = chuj.OrderBy(x => x);
        //var top3ElvesCalories = _elves.OrderByDescending(x=> x.Amount).Take(3).Sum(x=> x.Amount);

        //Console.WriteLine($"Top 3 Elves carry {top3ElvesCalories} calories total.");
    }
    

   

   

}   