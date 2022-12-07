using System.Reflection;

namespace Aoc;

public class Day1 : BaseDay
{
    List<Elf> _elves = new();

    protected override void ReadFile()
    {
        var elfNo = 1;
        using var stream = File.OpenRead("input.txt");
        using var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (string.IsNullOrEmpty(line) || _elves.Count == 0)
            {
                _elves.Add(new Elf(elfNo++, 0));
                continue;
            }
            var elf = _elves[elfNo - 2];
            elf.Amount += Convert.ToInt32(line);
        }
    }

    protected override void Part1()
    {
        var theBestElf = _elves.MaxBy(x=> x.Amount);

        Console.WriteLine($"The Best Elf: {theBestElf.Number}. Ammount: {theBestElf.Amount}");
    }
    
    protected override void Part2()
    {
        var top3ElvesCalories = _elves.OrderByDescending(x=> x.Amount).Take(3).Sum(x=> x.Amount);

        Console.WriteLine($"Top 3 Elves carry {top3ElvesCalories} calories total.");
    }
    

   

   

}