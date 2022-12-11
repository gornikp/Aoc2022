using System.Collections.Immutable;
using System.Diagnostics.Tracing;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Aoc;

public class Day10 : BaseDay
{
    private List<string> _data = new List<string>();

    protected override void ReadFile()
    {
        _data = File.ReadAllLines("Day10.txt").ToList();
    }

    protected override void Part1()
    {
        int x = 1;
        Console.WriteLine(File.ReadAllText("Day10.txt").Split(new[] { Environment.NewLine, " " }, 0)
            .Select((x, i) => (index: i, addrx: int.TryParse(x, out int parsed) ? parsed : 0))
            .Select(y => (y.index, addrx: x += y.addrx))
            .Where(y => y.index % 40 == 19)
            .Sum(y => y.addrx * (y.index + 1)));
            
        var registerX = ProcessInstructions();
        Console.WriteLine($"Part1: {registerX}");
    }

    protected override void Part2()
    {   
        int x = 1;
        var xd = string.Join(Environment.NewLine, File.ReadAllText("Day10.txt").Split(new[] { Environment.NewLine, " " }, 0)
            .Select((x, i) => (index: i, addrx: int.TryParse(x, out int parsed) ? parsed : 0))
            .Select(y => (index: y.index, addrx: x += y.addrx))
            .Select(y => x - 1 <= (y.index)%40 && (y.index)%40 <= x + 1 ? "█" : ".")
            .Chunk(40).Select(y => string.Join("", y)));

        Console.WriteLine(xd);
        var instructions = GetInstructions();
        var table = Enumerable.Range(0, 6).Select(x => Enumerable.Range(0, 40).Select(y => '.').ToList()).ToList();
        int clock = 0;
        int RegisterX = 1;
        
        while (instructions.Count > 0)
        {
            var instr = instructions.Peek();
            if (instr.Name == "noop")
                instructions.Dequeue();
            else if (instr.Clock > 1)
                instr.Clock--;
            else
            {
                instr = instructions.Dequeue();
                RegisterX += instr.Value;
            }

            var index = (int)Math.Floor((double)clock / 40);
            var index2 = (clock) % 40;
            if (RegisterX + 1 >= index2 && index2 >= RegisterX - 1)
            {
                table[index][index2] = '█';
            }

            clock++;
        }

        foreach (var line in table)
        {
            Console.WriteLine(string.Concat(line));
        }
    }

    private int ProcessInstructions()
    {
        var instructions = GetInstructions();

        int clock = 0;
        var cycles = new int[] { 20, 60, 100, 140, 180, 220 };
        int RegisterX = 1;
        int signalStrength = 0;
        while (instructions.Count > 0)
        {
            if (cycles.Contains(clock))
                signalStrength += clock * RegisterX;
            
            var instr = instructions.Peek();
            if (instr.Name == "noop")
                instructions.Dequeue();
            else if (instr.Clock > 1)
                instr.Clock--;
            else
            {
                instr = instructions.Dequeue();
                RegisterX += instr.Value;
            }
            clock++;
        }

        Console.WriteLine($"Clock: {clock}");
        return signalStrength;
    }

    private Queue<AddX> GetInstructions()
    {
        Queue<AddX> instructions = new Queue<AddX>();
        foreach (var line in _data)
        {
            var instr = line.Split(" ");
            if (instr[0] == "addx")
                instructions.Enqueue(new(instr[0], 2, int.Parse(instr[1])));
            else
                instructions.Enqueue(new(instr[0], 1, 0));
        }

        return instructions;
    }

    private sealed class AddX
    {
        public AddX(string name, int clock, int value)
        {
            Name = name;
            Clock = clock;
            Value = value;
        }

        public string Name { get; set; }
        public int Clock { get; set; }
        public int Value { get; set; }
    }
}