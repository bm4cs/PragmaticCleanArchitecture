#!/usr/bin/env pwsh

<#
.SYNOPSIS
Cake build script bootstrap for Windows PowerShell and PowerShell Core
.DESCRIPTION
This script will restore tools and run the Cake build script.
.PARAMETER Target
The target to run (default: "Default")
.PARAMETER Configuration
The configuration to use (default: "Release")
.PARAMETER Verbosity
The verbosity level (default: "normal")
#>

[CmdletBinding()]
Param(
    [Parameter(ValueFromRemainingArguments=$true)]
    [string[]]$Arguments
)

Write-Host "Preparing to run build script..." -ForegroundColor Green

# Check if .NET is installed
$DotNetInstalled = $null
try {
    $DotNetInstalled = Get-Command dotnet -ErrorAction SilentlyContinue
} catch {
    # Ignore
}

if ($null -eq $DotNetInstalled) {
    throw ".NET SDK is required to run this script. Please install .NET SDK from https://dotnet.microsoft.com/download"
}

# Restore Cake tool
Write-Host "Restoring Cake tool..." -ForegroundColor Yellow
dotnet tool restore
if ($LASTEXITCODE -ne 0) {
    throw "Failed to restore .NET tools"
}

# Run Cake with all arguments passed through
Write-Host "Running Cake build script..." -ForegroundColor Yellow
if ($Arguments) {
    Write-Host "Command: dotnet cake build.cake $($Arguments -join ' ')" -ForegroundColor Gray
    & dotnet cake build.cake @Arguments
} else {
    Write-Host "Command: dotnet cake" -ForegroundColor Gray
    & dotnet cake
}
if ($LASTEXITCODE -ne 0) {
    throw "Cake build failed"
}

Write-Host "Build script completed successfully!" -ForegroundColor Green