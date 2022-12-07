using Advent2022.Shared;

var input = await InputReader.Read(typeof(Program).Assembly, ParseInput);

await Challenge.Part1(spinner =>
{
    var count = 0;
    foreach (var (Elf1, Elf2) in input)
    {
        var (first, other) = (Elf1.Start == Elf2.Start && Elf1.End >= Elf2.End) || Elf1.Start < Elf2.Start ? (Elf1, Elf2) : (Elf2, Elf1);
        if (first.End >= other.End)
        {
            count++;
        }
    }

    return count;
});

await Challenge.Part2(spinner =>
{
    var count = 0;
    foreach (var (Elf1, Elf2) in input)
    {
        var (first, other) = (Elf1.Start == Elf2.Start && Elf1.End >= Elf2.End) || Elf1.Start < Elf2.Start ? (Elf1, Elf2) : (Elf2, Elf1);
        if (other.Start <= first.End)
        {
            count++;
        }
    }

    return count;
});

static IEnumerable<ElfPair> ParseInput(string input)
{
    foreach (var line in input.Split(Environment.NewLine))
    {
        var parts = line
            .Split(',')
            .Select(s => s
                .Split('-')
                .Select(int.Parse)
                .ToArray())
            .Select(s => new Assignment(s[0], s[1]))
            .ToArray();

        yield return new ElfPair(parts[0], parts[1]);
    }
}

record Assignment(int Start, int End);

record ElfPair(Assignment Elf1, Assignment Elf2);
