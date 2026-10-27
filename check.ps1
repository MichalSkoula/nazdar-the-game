<#
.SYNOPSIS
    Runs the automated tests and checks that the game builds, without leaving builds on disk.

.DESCRIPTION
    1. Runs the unit tests in Nazdar.Tests.
    2. Builds Nazdar.DX and Nazdar.GL (Release), including the content (Content.mgcb), so a broken
       build or a missing asset shows up. The builds go to a temporary folder, which is deleted
       afterwards; your builds in bin/ are not touched, so this can run while Visual Studio is open.
       The compiled content cache in Nazdar.Shared\Content\bin and obj is shared with Visual Studio,
       so only changed assets are rebuilt.
    Android, UWP and the Store packaging are built from Visual Studio and are not checked here.
    Only errors are printed for the builds; warnings are visible in Visual Studio.
    Exits with 1 when anything fails.

.PARAMETER SkipBuild
    Runs only the tests.

.PARAMETER KeepOutput
    Keeps the temporary builds (the path is printed); clean.ps1 deletes them later.

.PARAMETER Clean
    Runs clean.ps1 afterwards to delete outdated builds.

.EXAMPLE
    .\check.ps1             # tests and builds
    .\check.ps1 -SkipBuild  # tests only
    .\check.ps1 -Clean      # tests, builds, then delete outdated builds
#>
param(
    [switch]$SkipBuild,
    [switch]$KeepOutput,
    [switch]$Clean
)

$ErrorActionPreference = 'Stop'

# must match the folder in clean.ps1
$output = Join-Path ([IO.Path]::GetTempPath()) 'nazdar-check'

$steps = [ordered]@{}

function Invoke-Step([string]$name, [scriptblock]$command) {
    Write-Host ''
    Write-Host "=== $name" -ForegroundColor Cyan
    $watch = [Diagnostics.Stopwatch]::StartNew()
    & $command
    $ok = $LASTEXITCODE -eq 0
    $script:steps[$name] = [pscustomobject]@{ Ok = $ok; Seconds = [int]$watch.Elapsed.TotalSeconds }
}

Push-Location $PSScriptRoot
try {
    Invoke-Step 'Tests' {
        dotnet test Nazdar.Tests\Nazdar.Tests.csproj --nologo --verbosity quiet
    }

    if (-not $SkipBuild) {
        Invoke-Step 'Build Nazdar.DX' {
            dotnet build Nazdar.DX\Nazdar.DX.csproj -c Release --nologo --verbosity quiet -consoleLoggerParameters:ErrorsOnly --artifacts-path $output
        }

        # x64 is the platform Visual Studio builds for Linux and Windows releases
        Invoke-Step 'Build Nazdar.GL' {
            dotnet build Nazdar.GL\Nazdar.GL.csproj -c Release -p:Platform=x64 --nologo --verbosity quiet -consoleLoggerParameters:ErrorsOnly --artifacts-path $output
        }
    }
}
finally {
    Pop-Location

    if (Test-Path -LiteralPath $output) {
        if ($KeepOutput) {
            Write-Host ''
            Write-Host "Builds kept in $output"
        }
        else {
            Remove-Item -LiteralPath $output -Recurse -Force -ErrorAction SilentlyContinue
        }
    }
}

Write-Host ''
Write-Host '=== Summary' -ForegroundColor Cyan
foreach ($step in $steps.GetEnumerator()) {
    if ($step.Value.Ok) {
        Write-Host ('  OK    {0} ({1} s)' -f $step.Key, $step.Value.Seconds) -ForegroundColor Green
    }
    else {
        Write-Host ('  FAIL  {0} ({1} s)' -f $step.Key, $step.Value.Seconds) -ForegroundColor Red
    }
}

$failed = @($steps.Values | Where-Object { -not $_.Ok }).Count -gt 0

if ($Clean) {
    Write-Host ''
    & (Join-Path $PSScriptRoot 'clean.ps1')
}

if ($failed) {
    exit 1
}
