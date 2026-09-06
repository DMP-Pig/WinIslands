<#
.SYNOPSIS
    Builds a distributable WinIslands package.

.DESCRIPTION
    - Self-contained (default): includes the .NET 8 runtime, runs anywhere.
    - Framework-dependent (-FrameworkDependent): tiny, needs .NET 8 Desktop Runtime.
    Optionally creates a .zip of the publish output.

.EXAMPLE
    .\build\publish.ps1                     # self-contained -> publish\win-x64
    .\build\publish.ps1 -FrameworkDependent # framework-dependent
    .\build\publish.ps1 -SkipZip
#>
param(
    [switch]$FrameworkDependent,
    [switch]$SkipZip,
    [string]$OutputDir = ""
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$proj = Join-Path $root 'src\WinIslands\WinIslands.csproj'
if ($OutputDir -eq "") { $OutputDir = Join-Path $root 'publish' }

$dotnet = Join-Path $env:USERPROFILE '.dotnet\dotnet.exe'
if (-not (Test-Path $dotnet)) { $dotnet = 'dotnet' }

$rid = 'win-x64'
$args = @('publish', $proj, '-c', 'Release', '-r', $rid)
if ($FrameworkDependent) {
    $args += @('--self-contained', 'false')
} else {
    $args += @('--self-contained', 'true')
}
$out = Join-Path $OutputDir $rid
$args += @('-o', $out)

Write-Host "==> dotnet $($args -join ' ')"
& $dotnet @args
if ($LASTEXITCODE -ne 0) { throw "publish failed" }

if (-not $SkipZip) {
    $zip = Join-Path $OutputDir "WinIslands-$rid.zip"
    if (Test-Path $zip) { Remove-Item $zip -Force }
    Compress-Archive -Path (Join-Path $out '*') -DestinationPath $zip -CompressionLevel Optimal
    Write-Host "==> zip created: $zip"
}

Write-Host "==> output: $out"
