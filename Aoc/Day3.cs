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
        var commons = GetCommponPart2();
        var sum = commons.Sum(x => char.IsUpper(x) ? x - 38 : x - 96 );
        Console.WriteLine($"Tum of badges {sum}.");
    }

    private IEnumerable<char> GetCommponPart2()
    {
        for (int i = 0; i < _compartments.Count; i += 3)
        {
            var elf1 = _compartments[i].CompartmentA + _compartments[i].CompartmentB;
            var elf2 = _compartments[i + 1].CompartmentA + _compartments[i + 1].CompartmentB;
            var elf3 = _compartments[i + 2].CompartmentA + _compartments[i + 2].CompartmentB;

            yield return elf1.Where(elf2.Contains).Where(elf3.Contains).Distinct().First();
        }
    }
}   