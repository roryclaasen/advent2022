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
    <PackageReference Include="Kurukuru" Version="`$(KurukuruVersion)" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="`$(SolutionDir)Advent2022.Shared\Advent2022.Shared.csproj" />
  </ItemGroup>

</Project>
"@

Set-Content -Path (Join-Path $ProjectOutput "Program.cs") -Value @"
using Advent2022.Shared;
using Kurukuru;

var inputReader = new InputReader(typeof(Program).Assembly);
var input = inputReader.ReadFile("input.txt");

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
"@

dotnet restore $SolutionFile
