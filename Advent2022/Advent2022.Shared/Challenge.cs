namespace Advent2022.Shared
{
    using Kurukuru;
    using System.Diagnostics;

    public static class Challenge
    {
        public static void Part1<T>(Func<Spinner, T?> solution) => Part1(spinner => Task.FromResult(solution(spinner)));

        public static Task Part1<T>(Func<Spinner, Task<T?>> solution) => Solve(1, solution);

        public static void Part2<T>(Func<Spinner, T?> solution) => Part2(spinner => Task.FromResult(solution(spinner)));

        public static Task Part2<T>(Func<Spinner, Task<T?>> solution) => Solve(2, solution);

        private static Task Solve<T>(int part, Func<Spinner, Task<T?>> solution)
        {
            var prefix = $"Part {part}";
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return Spinner.StartAsync($"{prefix}: Solving", async spinner =>
               {
                   try
                   {
                       var result = await solution(spinner).ConfigureAwait(false);
                       stopwatch.Stop();
                       if (!spinner.Stopped)
                       {
                           if (result is null)
                           {
                               spinner.Warn($"{prefix}: ({stopwatch.ElapsedMilliseconds}ms): Answer returned is null");
                           }
                           else
                           {
                               spinner.Succeed($"{prefix}: ({stopwatch.ElapsedMilliseconds}ms): {result}");
                           }
                       }
                   }
                   catch (Exception ex)
                   {
                       stopwatch.Stop();
                       spinner.Fail($"{prefix}: ({stopwatch.ElapsedMilliseconds}ms): {ex.Message}");
                       throw;
                   }
               });
        }
    }
}
