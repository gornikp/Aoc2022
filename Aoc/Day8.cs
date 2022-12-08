using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Aoc;

public class Day8 : BaseDay
{
    private List<string> _data = new List<string>();

    protected override void ReadFile()
    {
        using var stream = File.OpenRead("Day8.txt");
        using var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line is null) continue;
            _data.Add(line);
        }
    }

    public class Tree
    {
        public Tree(short height, bool isVisibleTop, bool isVisibleBottom, bool isVisibleLeft, bool isVisibleRight)
        {
            Height = height;
            IsVisibleTop = isVisibleTop;
            IsVisibleBottom = isVisibleBottom;
            IsVisibleLeft = isVisibleLeft;
            IsVisibleRight = isVisibleRight;
        }

        public short Height { get; set; }
        public bool IsVisibleTop { get; set; }
        public bool IsVisibleBottom { get; set; }
        public bool IsVisibleLeft { get; set; }
        public bool IsVisibleRight { get; set; }

        public int VisibilityTop { get; set; }
        public int VisibilityBottom { get; set; }
        public int VisibilityLeft { get; set; }
        public int VisibilityRight { get; set; }
    }

    private List<List<Tree>> WriteIntoArray()
    {
        var list = new List<List<Tree>>();
        for (var i = 0; i < _data.Count; i++)
        {
            var line = _data[i];
            var row = new List<Tree>();
            for (var j = 0; j < line.ToList().Count; j++)
            {
                var value = line.ToList()[j];
                row.Add(new(short.Parse(value.ToString()), i == 0, i == _data.Count - 1, j == 0, j == line.ToList().Count - 1));
            }

            list.Add(row);
        }


        return list;
    }

    protected override void Part1()
    {
        var array = WriteIntoArray();

        var count = 0;
        var tempHighestL = new short[array.Count];
        var tempHighestT = new short[array[0].Count];

        for (int i = 0; i < array.Count; i++)
        {
            for (int j = 0; j < array[i].Count; j++)
            {
                var current = array[i][j];
                current.IsVisibleLeft = tempHighestL[i] < current.Height || current.IsVisibleLeft;
                current.IsVisibleTop = tempHighestT[j] < current.Height || current.IsVisibleTop;

                tempHighestL[i] = current.IsVisibleLeft ? current.Height : tempHighestL[i];
                tempHighestT[j] = current.IsVisibleTop ? current.Height : tempHighestT[j];
            }
        }

        var tempHighestR = new short[array.Count];
        var tempHighestB = new short[array[0].Count];
        for (int i = array.Count - 1; i > -1; i--)
        {
            for (int j = array[i].Count - 1; j > -1; j--)
            {
                var current = array[i][j];
                current.IsVisibleRight = tempHighestR[i] < current.Height || current.IsVisibleRight;
                current.IsVisibleBottom = tempHighestB[j] < current.Height || current.IsVisibleBottom;

                tempHighestR[i] = current.IsVisibleRight ? current.Height : tempHighestR[i];
                tempHighestB[j] = current.IsVisibleBottom ? current.Height : tempHighestB[j];

                if (current.IsVisibleLeft || current.IsVisibleTop || current.IsVisibleBottom || current.IsVisibleRight)
                    count++;
            }
        }

        Console.WriteLine($"Part1: {count}");
    }

    protected override void Part2()
    {
        var array = WriteIntoArray();
        for (int i = 0; i < array.Count; i++)
        {
            for (int j = 0; j < array[i].Count; j++)
            {
                var current = array[i][j];
                foreach (var tree in GetAllLeft(array, i, j))
                {
                    current.VisibilityLeft++;
                    if (current.Height <= tree.Height)
                        break;
                }
                foreach (var tree in GetAllRight(array, i, j))
                {
                    current.VisibilityRight++;
                    if (current.Height <= tree.Height)
                        break;
                }
                foreach (var tree in GetAllTop(array, i, j))
                {
                    current.VisibilityTop++;
                    if (current.Height <= tree.Height)
                        break;
                }
                foreach (var tree in GetAllBottom(array, i, j))
                {
                    current.VisibilityBottom++;
                    if (current.Height <= tree.Height)
                        break;
                }
            }
        }

        var output = array.SelectMany(x => x.Select(y => y)).Max(y=> y.VisibilityLeft * y.VisibilityBottom * y.VisibilityRight * y.VisibilityTop);
        Console.WriteLine($"Part2: {output}");
    }

    private IEnumerable<Tree> GetAllLeft(List<List<Tree>> array, int i, int j)
    {
        return array[i].Take(j).Reverse();
    }

    private IEnumerable<Tree> GetAllRight(List<List<Tree>> array, int i, int j)
    {
        return array[i].Skip(j + 1);
    }

    private IEnumerable<Tree> GetAllTop(List<List<Tree>> array, int i, int j)
    {
        return array.Select(x => x[j]).Take(i).Reverse();
    }

    private IEnumerable<Tree> GetAllBottom(List<List<Tree>> array, int i, int j)
    {
        return array.Select(x => x[j]).Skip(i + 1);
    }
}