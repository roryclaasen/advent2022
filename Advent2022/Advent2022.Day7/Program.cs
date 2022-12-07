using Advent2022.Shared;
using System.Text.RegularExpressions;

var input = await InputReader.Read(typeof(Program).Assembly, ParseInput).ConfigureAwait(false);

Challenge.Part1(spinner =>
{
    var total = 0;

    void DirectorySize(Day7.Directory dir)
    {
        var size = dir.Size;
        if (size <= 100000)
        {
            total += size;
        }

        foreach (var (_, node) in dir.Nodes)
        {
            if (node is Day7.Directory nodeDir)
            {
                DirectorySize(nodeDir);
            }
        }
    }

    DirectorySize(input);
    return total;
});

Challenge.Part2(spinner =>
{
    var currentFree = 70000000 - input.Size;
    var neededSize = 30000000;
    var possibleDirs = new List<Day7.Directory>();

    void DirectoryScan(Day7.Directory dir)
    {
        var size = dir.Size;
        if (currentFree + size >= neededSize)
        {
            possibleDirs.Add(dir);
        }

        foreach (var (_, node) in dir.Nodes)
        {
            if (node is Day7.Directory nodeDir)
            {
                DirectoryScan(nodeDir);
            }
        }
    }

    DirectoryScan(input);
    return possibleDirs.OrderBy(d => d.Size).First().Size;
});

static Day7.Directory ParseInput(string input)
{
    var cmdRegex = new Regex(@"^\$ (cd|ls)(?: (\w+|..|\/))?$");
    var listRegex = new Regex(@"^(dir|\d+) (\w+\.?\w*)$");

    var lines = new Queue<string>(input.Split(Environment.NewLine));
    Day7.Directory? tree = null;
    Day7.Directory? currentDir = null;
    while (lines.Count > 0)
    {
        var instruction = lines.Dequeue();
        var insRegex = cmdRegex.Match(instruction);
        var ins = insRegex.Groups[1].Value;
        if (ins == "cd")
        {
            var dir = insRegex.Groups[2].Value;
            if (dir == "..")
            {
                if (currentDir?.Parent is Day7.Directory parent)
                {
                    currentDir = parent;
                }
                else
                {
                    throw new Exception("Unable to go back a layer");
                }
            }
            else
            {
                if (currentDir is null)
                {
                    var node = new Day7.Directory(null, dir);
                    tree = node;
                    currentDir = node;
                }
                else
                {
                    if (currentDir.Nodes.TryGetValue(dir, out var node))
                    {
                        if (node is Day7.Directory directory)
                        {
                            currentDir = directory;
                        }
                        else
                        {
                            throw new Exception("Node is not a directory");
                        }
                    }
                    else
                    {
                        var newNode = new Day7.Directory(currentDir, dir);
                        currentDir.Nodes.Add(dir, newNode);
                        currentDir = newNode;
                    }
                }
            }
        }
        else if (ins == "ls")
        {
            while (lines.Count > 0 && !lines.Peek().StartsWith("$"))
            {
                var output = lines.Dequeue();
                var outRegex = listRegex.Match(output);
                var first = outRegex.Groups[1].Value;
                var second = outRegex.Groups[2].Value;
                Day7.Node? node;
                if (first == "dir")
                {
                    node = new Day7.Directory(currentDir, second);
                }
                else
                {
                    node = new Day7.File(currentDir, second, int.Parse(first));
                }

                if (node is null)
                {
                    throw new Exception("Unable to parse node");
                }
                currentDir.Nodes.Add(second, node);
            }
        }
        else
        {
            throw new Exception($"Unknown command {ins}");
        }
    }

    if (tree is null)
    {
        throw new Exception("Unable to parse tree");
    }

    return tree;
}

namespace Day7
{
    public abstract class Node
    {
        public Node Parent { get; }

        public string Name { get; }

        public virtual int Size { get; }

        public Node(Node parent, string name)
        {
            this.Parent = parent;
            this.Name = name;
        }
    }

    public class File : Node
    {
        public override int Size { get; }

        public File(Directory parent, string name, int size) : base(parent, name)
        {
            this.Size = size;
        }
    }

    public class Directory : Node
    {
        public Dictionary<string, Node> Nodes { get; private set; } = new();

        public override int Size => this.Nodes.Values.Sum(f => f.Size);

        public Directory(Directory parent, string name) : base(parent, name)
        {
        }
    }
}
