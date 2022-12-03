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
        score += Priority(elves[i].Everything
            .Distinct()
            .Where(letter => elves[i + 1].Everything.Contains(letter) && elves[i + 2].Everything.Contains(letter))
            .Single());
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
