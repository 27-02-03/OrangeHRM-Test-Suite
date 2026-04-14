# --- OrangeHRM Playwright & Allure Automation Script ---
$ErrorActionPreference = "Continue"

Write-Host "`n--- 1. BUILDING PROJECT ---" -ForegroundColor Cyan
dotnet build

Write-Host "`n--- 2. REFRESHING BROWSER BINARIES ---" -ForegroundColor Magenta
# This ensures Chromium is installed in your AppData for Playwright to find
dotnet run --project OrangeHRM.Tests.csproj -- playwright install chromium

Write-Host "`n--- 3. EXECUTING NUNIT TESTS ---" -ForegroundColor Yellow
# Runs the tests. Even if they fail, the script will move to the report step.
dotnet test --no-build

Write-Host "`n--- 4. GENERATING ALLURE REPORT ---" -ForegroundColor Green
# This is the path where your .NET 10 project outputs the Allure JSON files
$resultsPath = "bin\Debug\net10.0\allure-results"

if (Test-Path $resultsPath) {
    Write-Host "Results found! Launching Allure via NPX..." -ForegroundColor Cyan
    # Using npx ensures we don't need to worry about the Windows System Path
    npx allure-commandline serve $resultsPath
}
else {
    Write-Host "ERROR: No allure-results folder found at $resultsPath" -ForegroundColor Red
    Write-Host "Check if [AllureNUnit] attribute is added to your Test Class." -ForegroundColor Yellow
}

Write-Host "`n--- Process Finished ---" -ForegroundColor Gray
Pause