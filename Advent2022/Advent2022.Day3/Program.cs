using Advent2022.Shared;

var input = await InputReader.ReadAndParse(typeof(Program).Assembly, ParseInput).ConfigureAwait(false);

Challenge.Part1(spinner => input
    .Select(rucksack => rucksack.Compartment1
        .Distinct()
        .Where(letter => rucksack.Compartment2.Contains(letter))
        .Select(Priority)
        .Sum()
    ).Sum()
);

Challenge.Part2(spinner =>
{
    var elves = input.ToArray();
    var score = 0;
    for (var i = 0; i < elves.Length; i += 3)
    {
        var elf1 = elves[i];
        var elf2 = elves[i + 1];
        var elf3 = elves[i + 2];

        char? shared = null;
        foreach (var letter in elf1.Everything.Distinct())
        {
            if (elf2.Everything.Contains(letter) && elf3.Everything.Contains(letter))
            {
                shared = letter;
                break;
            }
        }
        
        if (shared is null)
        {
            throw new Exception("No shared letter found");
        }

        score += Priority(shared.Value);
    }

    return score;
});

static IEnumerable<Rucksack> ParseInput(string input)
{
    foreach (var line in input.Split(Environment.NewLine))
    {
        var mid = line.Length / 2;
        yield return new Rucksack(line, line.Substring(0, mid), line.Substring(mid, mid));
    }
}

static int Priority(char item)
{
    var id = Convert.ToInt32(item);
    return id >= 97 ? id - 96 : id - 38;
}

record Rucksack(string Everything, string Compartment1, string Compartment2);
