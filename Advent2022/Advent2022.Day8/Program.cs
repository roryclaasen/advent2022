using Advent2022.Shared;

var input = await InputReader.Read(typeof(Program).Assembly, ParseInput);

await Challenge.Part1(spinner =>
{
    List<Cell> VisibleTrees(int x, int y, Direction direction)
    {
        var trees = new List<Cell>();

        var line = GetInputLine(x, y, direction);
        var start = line[0];

        var tallest = start;
        for (var i = 1; i < line.Count; i++)
        {
            var cell = line[i];

            if (cell.Value > tallest.Value)
            {
                tallest = cell;
                trees.Add(cell);
            }
        }

        trees.Add(start);

        return trees;
    }

    var visibleTrees = new List<Cell>();

    for (var x = 0; x < input.Width; x++)
    {
        visibleTrees.AddRange(VisibleTrees(x, 0, Direction.South));
        visibleTrees.AddRange(VisibleTrees(x, input.Height - 1, Direction.North));
    }

    for (var y = 0; y < input.Height; y++)
    {
        visibleTrees.AddRange(VisibleTrees(0, y, Direction.East));
        visibleTrees.AddRange(VisibleTrees(input.Width - 1, y, Direction.West));
    }

    return visibleTrees.Distinct().Count();
});

await Challenge.Part2(spinner =>
{
    int VisibleTrees(int x, int y, Direction direction)
    {
        var trees = new List<Cell>();

        var line = GetInputLine(x, y, direction);
        var start = line[0];

        var tallest = start;
        for (var i = 1; i < line.Count; i++)
        {
            var cell = line[i];

            if (cell.Value >= start.Value)
            {
                tallest = cell;
                trees.Add(cell);
                break;
            }

            if (cell.Value > tallest.Value)
            {
                tallest = cell;
                trees.Add(cell);
            }
            else if (tallest == start && cell.Value < start.Value)
            {
                trees.Add(cell);
            }
        }

        return trees.Count;
    }

    var inwardsOffset = 0;
    var scores = new List<int>();
    for (var x = inwardsOffset; x < input.Width - inwardsOffset; x++)
    {
        for (var y = inwardsOffset; y < input.Height - inwardsOffset; y++)
        {
            var north = VisibleTrees(x, y, Direction.North);
            var south = VisibleTrees(x, y, Direction.South);
            var east = VisibleTrees(x, y, Direction.East);
            var west = VisibleTrees(x, y, Direction.West);
            var score = north * south * east * west;
            scores.Add(score);
        }
    }

    return scores.Max();
});

static GridData ParseInput(string input)
{
    var lines = input.Split(Environment.NewLine).ToArray();
    var width = lines[0].Length;
    var height = lines.Length;

    Cell[,] grid = new Cell[width, height];

    for (var y = 0; y < height; y++)
    {
        var row = lines[y].ToArray().Select(s => s.ToString()).Select(int.Parse).ToArray();
        for (var x = 0; x < width; x++)
        {
            grid[x, y] = new Cell(x, y, row[x]);
        }
    }

    return new GridData(width, height, grid);
}

List<Cell> GetInputLine(int x, int y, Direction direction)
{
    var results = new List<Cell>();

    int xStep = 0, yStep = 0;
    if (direction == Direction.North || direction == Direction.South)
        yStep = direction == Direction.North ? -1 : 1;
    else
        xStep = direction == Direction.East ? 1 : -1;

    for (var i = 0; i < Math.Max(input!.Width, input!.Height); i++)
    {
        var cX = x + (xStep * i);
        var cY = y + (yStep * i);

        if (cX < 0 || cX >= input!.Width || cY < 0 || cY >= input!.Height)
        {
            break;
        }

        results.Add(input!.Grid[cX, cY]);
    }

    return results;
}

record GridData(int Width, int Height, Cell[,] Grid);

record Cell(int X, int Y, int Value);
