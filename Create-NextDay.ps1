$SolutionFolder = Join-Path $PSScriptRoot "Advent2022"
$SolutionFile = Join-Path $SolutionFolder "Advent2022.sln"

$TotalProjects = (Get-ChildItem -Path $SolutionFolder -Directory -Filter "Advent2022.Day*" | Measure-Object).Count

$ProjectNumber = $TotalProjects + 1
$ProjectName = "Advent2022.Day$ProjectNumber"
$ProjectOutput = Join-Path $SolutionFolder $ProjectName
$ProjectFile = Join-Path $ProjectOutput "$ProjectName.csproj"
$ProjectInputFile = Join-Path $ProjectOutput "input.txt"

Write-Host "Creating new project '$ProjectName' in '$SolutionFolder'"

dotnet new console -n $ProjectName -o $ProjectOutput --no-restore
dotnet sln $SolutionFile add $ProjectOutput  --solution-folder "Advent Solutions"

New-Item $ProjectInputFile -ItemType File

Set-Content -Path $ProjectFile -Value @"
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <EmbeddedResource Include="*.txt" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="`$(SolutionDir)Advent2022.Shared\Advent2022.Shared.csproj" />
  </ItemGroup>

</Project>
"@

Set-Content -Path (Join-Path $ProjectOutput "Program.cs") -Value @"
using Advent2022.Shared;

var input = await InputReader.Read(typeof(Program).Assembly).ConfigureAwait(false);

Challenge.Part1(spinner =>
{
    Thread.Sleep(500);
    return 13;
});

Challenge.Part2(spinner =>
{
    Thread.Sleep(500);
    spinner.Fail("Part 2: Something went wrong!");
    return string.Empty;
});
"@

dotnet restore $SolutionFile
