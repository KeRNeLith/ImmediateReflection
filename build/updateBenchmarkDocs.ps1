<#
.Synopsis
    Run the benchmarks (on .NET Framework 4.8 and .NET 8) and regenerate the result tables of the documentation
    in docs/documentation/benchmarks.

.Description
    1. Builds the benchmark project (Release, 64 bits).
    2. Runs every benchmark once (the executable covers both runtimes) and collects BenchmarkDotNet markdown reports.
    3. Rewrites the configuration block and the result tables of the benchmark documentation pages.
       Text around the tables (implementation details, conclusions...) is kept as is.

    The script can be run several times, tables are replaced, not duplicated.

.Parameter Fast
    Use short jobs: less accurate and still long (about 30 minutes for the whole suite, mostly process start/build costs). Do not use it to update the documentation.

.Parameter Filter
    Only run benchmarks matching this BenchmarkDotNet filter (example: "*FieldGetter*"). Documentation is NOT updated in this case
    because every report is needed. Useful to quickly test this script.

.Parameter SkipRun
    Do not build/run the benchmarks, only regenerate the documentation from existing reports (see ResultsPath).

.Parameter ResultsPath
    Folder containing the BenchmarkDotNet "*-report-github.md" files.
    Default: benchmarks/ImmediateReflection.Benchmark/BenchmarkDotNet.Artifacts/results

.Example
    ./build/updateBenchmarkDocs.ps1

.Example
    ./build/updateBenchmarkDocs.ps1 -Fast

.Example
    ./build/updateBenchmarkDocs.ps1 -Fast -Filter "*FieldGetter*"

.Example
    ./build/updateBenchmarkDocs.ps1 -SkipRun -ResultsPath C:\temp\results
#>
param(
[switch] $Fast,
[string] $Filter,
[switch] $SkipRun,
[string] $ResultsPath
)

$ErrorActionPreference = "Stop"

$rootDir = Split-Path -Parent $PSScriptRoot
$benchmarkDir = Join-Path $rootDir "benchmarks/ImmediateReflection.Benchmark"
$docsDir = Join-Path $rootDir "docs/documentation/benchmarks"
$artifactsDir = Join-Path $benchmarkDir "BenchmarkDotNet.Artifacts"
if ([string]::IsNullOrEmpty($ResultsPath))
{
    $ResultsPath = Join-Path $artifactsDir "results"
}

# Documentation pages and the benchmark classes (in order) providing the tables they contain
$pages = [ordered]@{
    "get_set_benchmarks.md"                   = @("FieldGetter", "FieldSetter", "PropertyGetter", "PropertySetter");
    "multiple_get_set_benchmarks.md"          = @("FieldMultiGetter", "FieldMultiSetter", "PropertyMultiGetter", "PropertyMultiSetter");
    "get_set_multiple_objects_benchmarks.md"  = @("PropertyGetterOverObjects", "PropertySetterOverObjects");
    "default_constructor_benchmarks.md"       = @("DefaultConstructor", "MultiDefaultConstructor");
    "copy_constructor_benchmarks.md"          = @("CopyConstructor", "MultiCopyConstructor");
    "attributes_benchmarks.md"                = @("GetAttributes");
}

# Jobs (id set by the benchmark project) and the title shown above each table
$jobs = @(
    @{ Id = ".NET 8.0"; Title = ".NET 8" },
    @{ Id = ".NET Framework 4.8"; Title = ".NET Framework 4.8" }
)

function Invoke-Benchmarks
{
    $project = Join-Path $benchmarkDir "ImmediateReflection.Benchmark.csproj"

    "Building benchmarks..."
    dotnet build $project -c Release -f net48
    if ($LASTEXITCODE -ne 0) { throw "Benchmark project build failed." }

    # The net48 executable runs benchmarks on both runtimes
    $exe = Join-Path $benchmarkDir "bin/Release/net48/ImmediateReflection.Benchmark.exe"

    if (Test-Path $artifactsDir)
    {
        Remove-Item $artifactsDir -Recurse -Force
    }

    $benchmarkArgs = @("--artifacts", $artifactsDir)
    if (-not [string]::IsNullOrEmpty($Filter))
    {
        $benchmarkArgs += @("--filter", $Filter)
    }
    if ($Fast)
    {
        $benchmarkArgs += "--fast"
    }

    "Running benchmarks (can take a while)..."
    & $exe @benchmarkArgs
    if ($LASTEXITCODE -ne 0) { throw "Benchmarks execution failed." }
}

function Read-Report
{
param(
[Parameter(Mandatory)]
[string] $ClassName )

    $file = Join-Path $ResultsPath "ImmediateReflection.Benchmark.$($ClassName)Benchmark-report-github.md"
    if (-not (Test-Path $file))
    {
        throw "Missing report $file."
    }

    $lines = Get-Content $file -Encoding UTF8
    $headerIndex = 0
    while ($headerIndex -lt $lines.Count -and -not $lines[$headerIndex].StartsWith("| Method"))
    {
        ++$headerIndex
    }
    if ($headerIndex -ge $lines.Count)
    {
        throw "No result table in $file."
    }

    $columns = @($lines[$headerIndex].Split("|") | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne "" })

    $rows = @()
    for ($i = $headerIndex + 2; $i -lt $lines.Count -and $lines[$i].StartsWith("|"); ++$i)
    {
        $cells = @($lines[$i].Split("|") | ForEach-Object { $_.Trim() })
        $cells = $cells[1..($cells.Count - 2)]
        if ([string]::IsNullOrEmpty($cells[0]))
        {
            continue
        }

        $row = @{}
        for ($c = 0; $c -lt $columns.Count; ++$c)
        {
            $row[$columns[$c]] = $cells[$c]
        }
        $rows += , $row
    }

    return $rows
}

function Get-ConfigurationBlock
{
    # Environment description is the same for every report, take the first one
    $file = Get-ChildItem $ResultsPath -Filter "*-report-github.md" | Select-Object -First 1
    $lines = Get-Content $file.FullName -Encoding UTF8

    $start = 0
    while ($start -lt $lines.Count -and -not $lines[$start].StartsWith("BenchmarkDotNet"))
    {
        ++$start
    }

    $block = @('```ini')
    for ($i = $start; $i -lt $lines.Count -and $lines[$i] -ne '```'; ++$i)
    {
        if (-not [string]::IsNullOrWhiteSpace($lines[$i]))
        {
            $block += $lines[$i]
        }
    }
    $block += '```'
    return $block
}

function Get-Cell
{
param(
[string] $Value,
[string] $Method )

    if ($Method -match "ByImmediate")
    {
        return "*$Value*"
    }
    if ($Method -match "Immediate")
    {
        return "**$Value**"
    }
    return $Value
}

function Get-ResultsBlock
{
param(
[Parameter(Mandatory)]
[string] $ClassName )

    $rows = Read-Report $ClassName
    $block = @()
    foreach ($job in $jobs)
    {
        $jobRows = @($rows | Where-Object { $_["Job"] -eq $job.Id -and $_["Mean"] -ne "NA" })
        if ($jobRows.Count -eq 0)
        {
            throw "No result for $($job.Id) in $ClassName benchmark."
        }

        $block += "**$($job.Title)**"
        $block += ""
        $block += "| Method | Mean | Error | StdDev | Ratio | RatioSD |"
        $block += "|--- |---:|---:|---:|---:|---:|"
        foreach ($row in $jobRows)
        {
            $method = $row["Method"]
            $cells = @("Method", "Mean", "Error", "StdDev", "Ratio", "RatioSD") | ForEach-Object { Get-Cell $row[$_] $method }
            $block += "| " + ($cells -join " | ") + " |"
        }
        $block += ""
    }
    return $block
}

function Test-RuntimeTitle
{
param(
[string] $Line )

    foreach ($job in $jobs)
    {
        if ($Line -eq "**$($job.Title)**")
        {
            return $true
        }
    }
    return $false
}

function Update-Page
{
param(
[string] $PageName,
[string[]] $Classes,
[string[]] $Configuration )

    $path = Join-Path $docsDir $PageName
    $bytes = [System.IO.File]::ReadAllBytes($path)
    $hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
    $text = [System.Text.Encoding]::UTF8.GetString($bytes)
    if ($hasBom)
    {
        $text = $text.Substring(1)
    }
    $useCrLf = $text.Contains("`r`n")
    $lines = $text -split "`r?`n"

    $output = New-Object System.Collections.Generic.List[string]
    $tableIndex = 0
    $configDone = $false
    $i = 0
    while ($i -lt $lines.Count)
    {
        $line = $lines[$i]

        if (-not $configDone -and $line.StartsWith('```'))
        {
            # Replace the first code block (configuration)
            ++$i
            while ($lines[$i] -ne '```')
            {
                ++$i
            }
            ++$i
            $output.AddRange($Configuration)
            $configDone = $true
        }
        elseif ($line.StartsWith("|") -or (Test-RuntimeTitle $line))
        {
            # Replace a results area: runtime titles and tables (previously generated or not) with their blank lines
            while ($i -lt $lines.Count -and ($lines[$i].StartsWith("|") -or (Test-RuntimeTitle $lines[$i]) -or ($lines[$i] -eq "" -and ($i + 1) -lt $lines.Count -and ($lines[$i + 1].StartsWith("|") -or (Test-RuntimeTitle $lines[$i + 1])))))
            {
                ++$i
            }

            # The generated block already ends with a blank line
            if ($i -lt $lines.Count -and $lines[$i] -eq "")
            {
                ++$i
            }

            if ($tableIndex -ge $Classes.Count)
            {
                throw "$PageName contains more tables than expected."
            }
            $output.AddRange([string[]](Get-ResultsBlock $Classes[$tableIndex]))
            ++$tableIndex
        }
        else
        {
            $output.Add($line)
            ++$i
        }
    }

    if ($tableIndex -ne $Classes.Count)
    {
        throw "$PageName contains $tableIndex tables but $($Classes.Count) were expected."
    }

    $eol = "`n"
    if ($useCrLf)
    {
        $eol = "`r`n"
    }
    $newText = $output -join $eol
    $encoding = New-Object System.Text.UTF8Encoding($hasBom)
    [System.IO.File]::WriteAllText($path, $newText, $encoding)
    "Updated $PageName."
}

if (-not $SkipRun)
{
    Invoke-Benchmarks
}

if (-not (Test-Path $ResultsPath))
{
    throw "Results folder $ResultsPath not found."
}

if (-not [string]::IsNullOrEmpty($Filter))
{
    "Filter used: documentation not updated. Reports are in $ResultsPath."
    return
}

$configuration = Get-ConfigurationBlock
foreach ($page in $pages.GetEnumerator())
{
    Update-Page $page.Key $page.Value $configuration
}
