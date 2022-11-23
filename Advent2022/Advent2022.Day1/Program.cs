using Kurukuru;

Spinner.Start("Part 1", spinner =>
{
    Thread.Sleep(500);
    spinner.Succeed("The answer is: 13");
});

Spinner.Start("Part 2", spinner =>
{
    Thread.Sleep(500);
    spinner.Fail("Something went wrong!");
});
