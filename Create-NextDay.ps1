$SolutionFolder = Join-Path $PSScriptRoot "Advent2022"
$SolutionFile = Join-Path $SolutionFolder "Advent2022.sln"

$TotalProjects = (Get-ChildItem -Path $SolutionFolder -Directory -Filter "Advent2022.Day*" | Measure-Object).Count
$NextProjectDay = $TotalProjects + 1

$NextProjectName = "Advent2022.Day$NextProjectDay"
$NextProjectOutput = Join-Path $SolutionFolder $NextProjectName

Write-Host "Creating new project '$NextProjectName' in '$SolutionFolder'"

dotnet new console -n $NextProjectName -o $NextProjectOutput
dotnet sln $SolutionFile add $NextProjectOutput
