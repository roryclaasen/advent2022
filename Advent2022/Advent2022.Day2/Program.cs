using Advent2022.Shared;

var input = await InputReader.ReadAndParse(typeof(Program).Assembly, input => input.Split(Environment.NewLine)).ConfigureAwait(false);

Challenge.Part1(spinner => CalculateScore(ParseInput1(input)));

Challenge.Part2(spinner =>
{
    var moves = new[] { RPS.Rock, RPS.Paper, RPS.Scissors };
    return CalculateScore(ParseInput2(input).Select(s =>
    {
        var move = moves.First(m => PlayGame(m, s.Opponent) == s.Result);
        return new RPSRound(move, s.Opponent);
    }));
});

static IEnumerable<RPSRound> ParseInput1(IEnumerable<string> input)
{
    foreach (var section in input)
    {
        var stat = section.Split(' ');
        var opponent = stat[0] == "A" ? RPS.Rock : stat[0] == "B" ? RPS.Paper : RPS.Scissors;
        var move = stat[1] == "X" ? RPS.Rock : stat[1] == "Y" ? RPS.Paper : RPS.Scissors;
        yield return new RPSRound(move, opponent);
    }
}

static IEnumerable<Strategy> ParseInput2(IEnumerable<string> input)
{
    foreach (var section in input)
    {
        var stat = section.Split(' ');
        var opponent = stat[0] == "A" ? RPS.Rock : stat[0] == "B" ? RPS.Paper : RPS.Scissors;
        var move = stat[1] == "X" ? Outcome.Lose : stat[1] == "Y" ? Outcome.Draw : Outcome.Win;
        yield return new Strategy(move, opponent);
    }
}

static int CalculateScore(IEnumerable<RPSRound> rounds)
{
    var totalScore = 0;
    foreach (var round in rounds)
    {
        totalScore += (int)round.Move;
        totalScore += PlayGame(round.Move, round.Opponent) switch
        {
            Outcome.Draw => 3,
            Outcome.Win => 6,
            Outcome.Lose => 0,
            _ => throw new NotImplementedException()
        };
    }

    return totalScore;
}

static Outcome PlayGame(RPS a, RPS b)
{
    if (a == b)
    {
        return Outcome.Draw;
    }

    return a switch
    {
        RPS.Rock => b == RPS.Scissors ? Outcome.Win : Outcome.Lose,
        RPS.Paper => b == RPS.Rock ? Outcome.Win : Outcome.Lose,
        RPS.Scissors => b == RPS.Paper ? Outcome.Win : Outcome.Lose,
        _ => throw new ArgumentOutOfRangeException(nameof(a), a, null)
    };
}

enum RPS : int { Rock = 1, Paper = 2, Scissors = 3 }
enum Outcome { Win, Lose, Draw }

record RPSRound(RPS Move, RPS Opponent);
record Strategy(Outcome Result, RPS Opponent);

