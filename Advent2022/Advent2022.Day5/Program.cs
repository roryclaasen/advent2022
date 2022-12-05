using Advent2022.Shared;
using System.Text.RegularExpressions;

var input = await InputReader.Read(typeof(Program).Assembly, ParseInput).ConfigureAwait(false);

Challenge.Part1(spinner =>
{
    var stacks = CloneStacks(input.Stacks).ToArray();
    foreach (var (Count, From, To) in input.Instructions)
    {
        for (var i = 0; i < Count; i++)
        {
            stacks[To - 1].Push(stacks[From - 1].Pop());
        }
    }

    return string.Join(string.Empty, stacks.Select(s => s.First().ToString()));
});

Challenge.Part2(spinner =>
{
    var stacks = CloneStacks(input.Stacks).ToArray();
    foreach (var (Count, From, To) in input.Instructions)
    {
        var items = new Stack<char>();
        for (var i = 0; i < Count; i++)
        {
            items.Push(stacks[From - 1].Pop());
        }

        foreach (var item in items)
        {
            stacks[To - 1].Push(item);
        }
    }

    return string.Join(string.Empty, stacks.Select(s => s.First().ToString()));
});

static IputData ParseInput(string input)
{
    var data = input.Split(Environment.NewLine + Environment.NewLine);
    return new IputData(ParseStack(data[0]).ToArray(), PasrseInstruction(data[1]));
}

static IEnumerable<Stack<char>> ParseStack(string input)
{
    var converted = input
        .Replace("] ", ",")
        .Replace(",[", ",")
        .Replace(" [", ",")
        .Replace(",   ,", ",,")
        .Replace(",       ,", ",,,")
        .Replace("[", string.Empty)
        .Replace("]", string.Empty)
        .Replace(" ", string.Empty)
        .Split(Environment.NewLine)
        .Select(l => l.Split(','));
    
    for (var i = 0; i < converted.Last().Single().Length; i++)
    {
        var stack = new Stack<char>();
        foreach (var line in converted.SkipLast(1).Reverse())
        {
            if (i < line.Length && !string.IsNullOrWhiteSpace(line[i]))
            {
                stack.Push(line[i].ToCharArray().First());
            }
        }
        yield return stack;
    }
}

static IEnumerable<Instruction> PasrseInstruction(string input)
{
    var regexMatch = new Regex(@"^move (\d+) from (\d+) to (\d+)$");

    foreach (var line in input.Split(Environment.NewLine))
    {
        var temp = regexMatch.Match(line);
        var groups = temp?.Groups;
        if (groups is null)
        {
            throw new ArgumentException($"Invalid input: {line}");
        }

        yield return new Instruction(int.Parse(groups[1].Value), int.Parse(groups[2].Value), int.Parse(groups[3].Value));
    }
}

static IEnumerable<Stack<char>> CloneStacks(IEnumerable<Stack<char>> stacks)
{
    foreach (var stack in stacks)
    {
        var newStack = new Stack<char>();
        foreach (var item in stack.Reverse())
        {
            newStack.Push(item);
        }
        yield return newStack;
    }
}

record IputData(IEnumerable<Stack<char>> Stacks, IEnumerable<Instruction> Instructions);

record Instruction(int Count, int From, int To);

