namespace Aoc;

public class Day4 : BaseDay
{
    private List<string> _data = new List<string>();
    protected override void ReadFile()
    {
        using var stream = File.OpenRead("Day4.txt");
        using var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line is null) continue;
            _data.Add(line);
        }
    }

    protected override void Part1()
    {
        // each line 94-96,23-95
        int count = 0;
        foreach (var pair in _data)
        {
            var splitted = pair.Split(',').Select(y => y.Split('-').Select(x=> int.Parse(x)).ToList()).ToList();
            var firstElf = Enumerable.Range(splitted[0][0], splitted[0][1] - splitted[0][0] + 1);
            var secondElf = Enumerable.Range(splitted[1][0], splitted[1][1] - splitted[1][0] + 1);

            if (firstElf.All(x => secondElf.Contains(x)) || secondElf.All(x => firstElf.Contains(x)))
            {
                count++;
            }
        }
        Console.WriteLine($"Part1: {count}");
    }

    protected override void Part2()
    {
        // each line 94-96,23-95
        int count = 0;
        foreach (var pair in _data)
        {
            var splitted = pair.Split(',').Select(y => y.Split('-').Select(x=> int.Parse(x)).ToList()).ToList();
            var firstElf = Enumerable.Range(splitted[0][0], splitted[0][1] - splitted[0][0] + 1);
            var secondElf = Enumerable.Range(splitted[1][0], splitted[1][1] - splitted[1][0] + 1);

            if (firstElf.Any(x => secondElf.Contains(x)) || secondElf.Any(x => firstElf.Contains(x)))
            {
                count++;
            }
        }
        Console.WriteLine($"Part2: {count}");
    }
}