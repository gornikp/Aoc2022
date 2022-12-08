using System.Net.Sockets;

namespace Aoc;

public class Day6 : BaseDay
{
    private List<string> _data = new List<string>();
    protected override void ReadFile()
    {
        using var stream = File.OpenRead("Day6.txt");
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
        Console.WriteLine($"Part1: {FindAllDistinctCharString(4)}");
    }

    private int FindAllDistinctCharString(int stringLength)
    {
        var listWithOverflow = new ListWithOverFlow<char>(stringLength);
        for (int i = 0; i < _data[0].Length; i++)
        {
            listWithOverflow.Add(_data[0][i]);

            if (stringLength == listWithOverflow.Distinct().Count())
            {
                return i + 1;
            }
        }
        return 0;
    }

    protected override void Part2()
    {
        Console.WriteLine($"Part1: {FindAllDistinctCharString(14)}");
    }
}



public class ListWithOverFlow<T> : List<T>
{
    public ListWithOverFlow(int capacity) : base(capacity) { }
    
    public void Add(T item)
    {
        if (Count == Capacity)
        {
            RemoveAt(0);
        }
        base.Add(item);
    }
}