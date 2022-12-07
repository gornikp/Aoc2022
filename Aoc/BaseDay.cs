namespace Aoc;

public abstract class BaseDay
{
    public void Execute()
    {
        ReadFile();
        Part1();
        Part2();
    }
    
    protected abstract void ReadFile();
    protected abstract void Part1();
    protected abstract void Part2();
}