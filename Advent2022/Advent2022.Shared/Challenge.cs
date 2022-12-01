namespace Advent2022.Shared
{
    using Kurukuru;

    public static class Challenge
    {
        public static void Part1<T>(Func<Spinner, T?> solution)
            => Part1Async(spinner => Task.FromResult(solution(spinner)));

        public static Task Part1Async<T>(Func<Spinner, Task<T?>> solution)
            => Solve(1, solution);

        public static void Part2<T>(Func<Spinner, T?> solution)
            => Part2Async(spinner => Task.FromResult(solution(spinner)));

        public static Task Part2Async<T>(Func<Spinner, Task<T?>> solution)
            => Solve(2, solution);

        private static Task Solve<T>(int part, Func<Spinner, Task<T?>> solution)
        {
            var prefix = $"Part {part}";
            return Spinner.StartAsync($"{prefix}: Solving", async spinner =>
               {
                   try
                   {
                       var result = await solution(spinner);
                       if (result is null)
                       {
                           spinner.Warn($"{prefix}: Answer returned is null");
                       }
                       else
                       {
                           spinner.Succeed($"{prefix}: {result}");
                       }
                   }
                   catch (Exception ex)
                   {
                       spinner.Fail($"{prefix}: {ex.Message}");
                   }
               });
        }
    }
}
