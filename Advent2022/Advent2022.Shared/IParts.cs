namespace Advent2022.Shared
{
    public interface IPart1<TInput>
    {
        public string SolvePart1(TInput input);
    }

    public interface IPart2<TInput>
    {
        public string SolvePart2(TInput input);
    }
}
