using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Aoc;

public class Day7 : BaseDay
{
    private List<string> _data = new List<string>();

    protected override void ReadFile()
    {
        using var stream = File.OpenRead("Day7.txt");
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
        var fs = FileSystem.Create(_data);
        var totalSize = fs.Main.GetAllDirs().Where(x => x.DirSize < 100000).Sum(x => x.DirSize);
        Console.WriteLine($"Part1: {totalSize}");
    }

    protected override void Part2()
    {
        var fs = FileSystem.Create(_data);
        var mimimumSpaceToFreeUp = 30000000 - fs.FreeSpace;
        var totalSize = fs.Main.GetAllDirs()
            .Where(x => x.DirSize > mimimumSpaceToFreeUp)
            .OrderBy(x => x.DirSize)
            .FirstOrDefault().DirSize;
        Console.WriteLine($"Part2: {totalSize}");
    }
}

public class FileSystem
{
    public static int TotalDiskSpace = 70_000_000;
    
    public FileSystem(Dir main)
    {
        Main = main;
        Current = main;
    }
    
     public static FileSystem Create(List<string> data)
    {
        var fs = new FileSystem(new Dir("/"));

        foreach (var line in data)
        {
            if (line.Contains("$ cd"))
            {
                var folder = line.Replace("$ cd ", "");
                if (folder == "..")
                    fs.Current = fs.Current.RootDir ?? fs.Current;
                else if (folder == "/")
                    fs.Current = fs.Main;
                else
                    fs.Current = fs.Current.Directories.First(x => x.Name == folder);
            }
            else if (line.Contains("$ ls")) continue;
            else if (line.Contains("dir"))
            {
                var folder = line.Replace("dir ", "");
                var dir = new Dir(folder, fs.Current);
                fs.Current.Directories.Add(dir);
            }
            else
            {
                var splitted = line.Split();
                fs.Current.Files.Add(new File2(splitted[1], fs.Current, int.Parse(splitted[0])));
            }
        }

        return fs;
    }

    public Dir Main { get; set; }
    public Dir Current { get; set; }
    public int FreeSpace => TotalDiskSpace - Main.DirSize;
}

public class Dir
{
    public Dir(string name, Dir? rootDir = null)
    {
        Name = name;
        RootDir = rootDir ?? null;
    }

    public string FullPath => RootDir is null ? Name : RootDir.FullPath + Name + '/';
    public Dir? RootDir { get; set; } = null;
    public string Name { get; set; }
    public List<Dir> Directories { get; set; } = new();
    public List<File2> Files { get; set; } = new();
    public int DirSize => Files.Sum(x => x.Size) + Directories.Sum(x => x.DirSize);

    public IEnumerable<Dir> GetAllDirs()
    {
        foreach (var dir in Directories)
        {
            yield return dir;

            foreach (var descendant in dir.GetAllDirs())
            {
                yield return descendant;
            }
        }
    }
}

public record File2(string Name, Dir RootDir, int Size)
{
    public string FullPath => RootDir.FullPath + Name;
}