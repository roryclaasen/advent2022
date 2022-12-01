using Advent2022.Shared;
using Kurukuru;

var inputReader = new InputReader(typeof(Program).Assembly);
var input = ParseInput(inputReader.ReadFile("input.txt"));

Spinner.Start("Part 1", spinner =>
{
    var elfs = input.ToArray();
    var max = elfs[0].Calories;
    var num = 0;
    for (var i = 1; i < elfs.Length; i++)
    {
        var cal = elfs[i].Calories;
        if (cal > max)
        {
            max = cal;
            num = i;
        }
    }

    spinner.Succeed($"Part 1: {max}");
});

Spinner.Start("Part 2", spinner =>
{
    var total = input
        .Select(e => e.Calories)
        .Order()
        .TakeLast(3)
        .Sum();

    spinner.Succeed($"Part 2: {total}");
});

static IEnumerable<Elf> ParseInput(string input)
{
    foreach (var section in input.Split(Environment.NewLine + Environment.NewLine))
    {
        yield return new Elf(section.Split(Environment.NewLine).Select(int.Parse).Sum());
    }
}

record Elf(int Calories);
