using Advent2022.Shared;
using Kurukuru;

var inputReader = new InputReader(typeof(Program).Assembly);
var input = ParseInput(inputReader.ReadFile("input.txt"));

Spinner.Start("Part 1", spinner =>
{
    var elfs = input.ToArray();
    var max = elfs[0].Calories.Sum();
    var num = 0;
    for (var i = 1; i < elfs.Length; i++)
    {
        var elf = elfs[i];
        var cal = elf.Calories.Sum();
        if (cal > max)
        {
            max = cal;
            num = i;
        }
    }

    spinner.Succeed($"The answer is: {max}");
});

Spinner.Start("Part 2", spinner =>
{
    var total = input
        .Select(e => e.Calories.Sum())
        .Order()
        .TakeLast(3)
        .Sum();
    spinner.Succeed($"The answer is: {total}");
});

IEnumerable<Elf> ParseInput(string input)
{
    foreach (var section in input.Split(Environment.NewLine + Environment.NewLine))
    {
        yield return new Elf(section.Split(Environment.NewLine).Select(int.Parse));
    }
}

record Elf(IEnumerable<int> Calories);
