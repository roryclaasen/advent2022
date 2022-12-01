using Advent2022.Shared;
using Kurukuru;

var inputReader = new InputReader(typeof(Program).Assembly);
var input = ParseInput(inputReader.ReadFile("input.txt"));

Spinner.Start("Part 1", spinner =>
{
    var max = input.MaxBy(e => e.Calories);
    if (max is null)
    {
        spinner.Fail("Part 1: Unable to find max calories");
        return;
    }

    spinner.Succeed($"Part 1: {max.Calories}");
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
