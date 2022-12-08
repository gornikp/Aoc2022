using System.Collections;
using System.Text.RegularExpressions;

namespace Aoc;

public class Day5 : BaseDay
{
    private List<string> _data = new List<string>();
    protected override void ReadFile()
    {
        using var stream = File.OpenRead("Day5.txt");
        using var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line is null) continue;
            _data.Add(line);
        }
    }

    private void FillStacks()
    {
        List<List<string>> lines = new List<List<string>>();
        var stringWithPlaceholders = _stacksString.Replace("    ", " [2]");
        var strReader = new StringReader(stringWithPlaceholders);

        foreach (var line in _data)
        {
            if (line.Contains("1"))
                break;
            
            var replaced = line.Replace("    ", " [2] ");
            lines.Add(replaced.Split().Where(x=> !string.IsNullOrEmpty(x) && x != " ").ToList());
        }

        for (int i = 0; i < 9; i++)
        {
            _stacks.Add(new());
        }

        lines.Reverse();
        foreach (var line in lines)
        {
            for (int i = 0; i < line.Count; i++)
            {
                if (!line[i].Contains("[2]"))
                    _stacks[i].Push(line[i]);
            }
        }
    }

    protected override void Part1()
    {
        FillStacks();
        foreach (var command in _data.Where(x=>x.StartsWith("move")))
        {
            //move 5 from 3 to 6
            Regex regex = new Regex(@"move ([\d]*) from ([\d]*) to ([\d]*)");
            var result = regex.Match(command);
            var move = int.Parse(result.Groups[1].Value);
            var from = int.Parse(result.Groups[2].Value) - 1;
            var to = int.Parse(result.Groups[3].Value) - 1;

            
            for (int i = 0; i < move; i++)
            {
                _stacks[to].Push(_stacks[from].Pop());
            }
        }

        string finalResult = "";
        foreach (var stack in _stacks.Where(x=>x.Any()))
        {
            finalResult += stack.Peek().Replace("[", "").Replace("]", "");
        }
        
        Console.WriteLine($"Part1: {finalResult}");
    }

    protected override void Part2()
    {
        FillStacks();
        foreach (var command in _data.Where(x=>x.StartsWith("move")))
        {
            //move 5 from 3 to 6
            Regex regex = new Regex(@"move ([\d]*) from ([\d]*) to ([\d]*)");
            var result = regex.Match(command);
            var move = int.Parse(result.Groups[1].Value);
            var from = int.Parse(result.Groups[2].Value) - 1;
            var to = int.Parse(result.Groups[3].Value) - 1;

            var tempList = new List<string>();
            for (int i = 0; i < move; i++)
            {
                tempList.Add(_stacks[from].Pop());
            }

            tempList.Reverse();
            tempList.ForEach(x=> _stacks[to].Push(x));
        }

        string finalResult = "";
        foreach (var stack in _stacks.Where(x=>x.Any()))
        {
            finalResult += stack.Peek().Replace("[", "").Replace("]", "");
        }
        
        Console.WriteLine($"Part2: {finalResult}");
    }
}