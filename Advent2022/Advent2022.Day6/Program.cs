using Advent2022.Shared;

var input = await InputReader.Read(typeof(Program).Assembly);

await Challenge.Part1(spinner => FindMarker(input, 4));

await Challenge.Part2(spinner => FindMarker(input, 14));

static int FindMarker(string input, int length)
{
    for (var i = 0; i < input.Length - length - 1; i++)
    {
        var marker = input.Substring(i, length);
        if (marker.Distinct().Count() == length)
        {
            return i + length;
        }
    }

    throw new Exception("Unbale to find marker");
}
