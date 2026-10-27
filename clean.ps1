<#
.SYNOPSIS
    Deletes old build outputs of all Nazdar.* projects, keeping the current ones.

.DESCRIPTION
    By default only outdated outputs are removed, so the current builds still run and can be uploaded:
      - bin/obj: target framework folders other than the newest one (e.g. net6.0 next to net10.0),
        for each configuration and platform separately (Debug and Release are both kept)
      - AppPackages/BundleArtifacts: package versions other than the newest one
      - the temporary output of check.ps1, if it was left behind
    With -All everything is removed (bin, obj, AppPackages, BundleArtifacts); it is all regenerated
    by the next build.
    Only Nazdar.* project folders are touched; todo/, output/, tmp/, .vs/ and the rest of the
    repository are left alone. Avoid `git clean -xdf`, it would also delete the ignored todo/ and
    output/ folders.

.PARAMETER All
    Deletes all build outputs, including the current builds.

.PARAMETER NuGet
    Also clears the global NuGet cache (shared by all .NET projects on this machine).
    Packages are downloaded again on the next restore.

.PARAMETER Force
    Runs even when Visual Studio is open. Visual Studio keeps stale build state after obj/
    is deleted and may skip the next build; restart it afterwards.

.EXAMPLE
    .\clean.ps1 -WhatIf     # show what would be deleted
    .\clean.ps1             # delete outdated build outputs, keep the current builds
    .\clean.ps1 -All        # delete all build outputs
    .\clean.ps1 -All -NuGet # delete all build outputs and clear NuGet cache
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [switch]$All,
    [switch]$NuGet,
    [switch]$Force
)

$ErrorActionPreference = 'Stop'

if (-not $Force -and -not $WhatIfPreference -and (Get-Process -Name devenv -ErrorAction SilentlyContinue)) {
    Write-Warning 'Visual Studio is running. Close it first (it would skip the next build), or use -Force and restart it afterwards.'
    return
}

$targetNames = @('bin', 'obj', 'AppPackages', 'BundleArtifacts')

# must match the output folder in check.ps1
$checkOutput = Join-Path ([IO.Path]::GetTempPath()) 'nazdar-check'

function Get-FolderSize([string]$path) {
    $sum = (Get-ChildItem -LiteralPath $path -Recurse -Force -File -ErrorAction SilentlyContinue | Measure-Object -Property Length -Sum).Sum
    if ($sum) { return [long]$sum } else { return 0 }
}

function Format-Size([long]$bytes) {
    if ($bytes -ge 1GB) { return '{0:N2} GB' -f ($bytes / 1GB) }
    return '{0:N0} MB' -f ($bytes / 1MB)
}

# when the item (or anything inside it) was last written; folder timestamps alone are unreliable
function Get-LastWrite($item) {
    $newest = $item.LastWriteTimeUtc
    if ($item.PSIsContainer) {
        $files = Get-ChildItem -LiteralPath $item.FullName -Recurse -Force -File -ErrorAction SilentlyContinue
        foreach ($file in $files) {
            if ($file.LastWriteTimeUtc -gt $newest) { $newest = $file.LastWriteTimeUtc }
        }
    }
    return $newest
}

# bin/obj: in every folder that holds target framework folders, all but the newest one are outdated
function Get-OutdatedFrameworkFolders([string]$path) {
    $folders = @(Get-ChildItem -LiteralPath $path -Recurse -Force -Directory -ErrorAction SilentlyContinue)
    $frameworks = $folders | Where-Object { $_.Name -match '^(net\d|netcoreapp|netstandard|uap)' }

    foreach ($group in ($frameworks | Group-Object { $_.Parent.FullName })) {
        if ($group.Count -lt 2) { continue }
        $group.Group | Sort-Object { Get-LastWrite $_ } -Descending | Select-Object -Skip 1 | ForEach-Object { $_.FullName }
    }
}

# AppPackages/BundleArtifacts: entries are named by package version (Nazdar_1.2.3.0_x86_Test);
# all entries of the newest version are kept, entries without a version are left alone
function Get-OutdatedPackages([string]$path) {
    $versioned = foreach ($item in (Get-ChildItem -LiteralPath $path -Force)) {
        if ($item.Name -match '_(\d+\.\d+\.\d+\.\d+)(_|\.|$)') {
            [pscustomobject]@{ Item = $item; Version = [version]$Matches[1] }
        }
    }

    $versions = @($versioned | Group-Object Version)
    if ($versions.Count -lt 2) { return }

    $newest = $versions | Sort-Object { ($_.Group | ForEach-Object { Get-LastWrite $_.Item } | Measure-Object -Maximum).Maximum } -Descending | Select-Object -First 1
    $versioned | Where-Object { $_.Version -ne [version]$newest.Name } | ForEach-Object { $_.Item.FullName }
}

# project folders and the shared content folder
$roots = @(Get-ChildItem -LiteralPath $PSScriptRoot -Directory -Filter 'Nazdar.*')
$sharedContent = Join-Path $PSScriptRoot 'Nazdar.Shared\Content'
if (Test-Path -LiteralPath $sharedContent) {
    $roots += Get-Item -LiteralPath $sharedContent
}

$targets = @(foreach ($root in $roots) {
    foreach ($name in $targetNames) {
        $path = Join-Path $root.FullName $name
        if (-not (Test-Path -LiteralPath $path)) { continue }

        if ($All) {
            $path
        }
        elseif ($name -eq 'bin' -or $name -eq 'obj') {
            Get-OutdatedFrameworkFolders $path
        }
        else {
            Get-OutdatedPackages $path
        }
    }
})

if (Test-Path -LiteralPath $checkOutput) {
    $targets += $checkOutput
}

if (-not $targets -and -not $NuGet) {
    if ($All) {
        Write-Host 'Nothing to clean.'
    }
    else {
        Write-Host 'Nothing outdated to clean. Use -All to delete the current builds too.'
    }
    return
}

$freed = 0
$failed = @()

foreach ($path in $targets) {
    if ($path.StartsWith($PSScriptRoot)) {
        $relative = $path.Substring($PSScriptRoot.Length).TrimStart('\')
    }
    else {
        $relative = $path
    }
    $size = Get-FolderSize $path

    if ($PSCmdlet.ShouldProcess("$relative ($(Format-Size $size))", 'Remove')) {
        try {
            Remove-Item -LiteralPath $path -Recurse -Force
            Write-Host ('Removed {0,-60} {1,10}' -f $relative, (Format-Size $size))
            $freed += $size
        }
        catch {
            # usually a file locked by Visual Studio or a running game
            Write-Warning "Could not fully remove $relative ($($_.Exception.Message))"
            $failed += $relative
            $freed += $size - (Get-FolderSize $path)
        }
    }
}

if ($NuGet -and $PSCmdlet.ShouldProcess('global NuGet cache', 'Clear')) {
    dotnet nuget locals all --clear
}

if (-not $WhatIfPreference) {
    Write-Host ''
    Write-Host "Freed: $(Format-Size $freed)"
    if ($failed.Count -gt 0) {
        Write-Host 'Some folders are locked; close Visual Studio and the game, then run the script again.'
    }
}
