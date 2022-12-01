using Advent2022.Shared;

var elves = await InputReader.ReadAndParse(typeof(Program).Assembly, ParseInput);

Challenge.Part1<int?>(spinner =>
{
    var max = elves.MaxBy(e => e.Calories);
    if (max is null)
    {
        spinner.Fail("Part 1: Unable to find max calories");
        return null;
    }

    return max.Calories;
});

Challenge.Part2(spinner =>
    elves
        .Select(e => e.Calories)
        .Order()
        .TakeLast(3)
        .Sum()
    );

static IEnumerable<Elf> ParseInput(string input)
{
    foreach (var section in input.Split(Environment.NewLine + Environment.NewLine))
    {
        yield return new Elf(section.Split(Environment.NewLine).Select(int.Parse).Sum());
    }
}

record Elf(int Calories);
