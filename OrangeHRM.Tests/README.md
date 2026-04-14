# OrangeHRM.Tests – .NET 10 C# automation

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- (Optional) If you don’t have .NET 10, change `<TargetFramework>net10.0</TargetFramework>` to `net8.0` in `OrangeHRM.Tests.csproj`.

## Run tests

From the repo root (`c:\Automation\orangehrm-automation`):

```bash
dotnet test OrangeHRMAutomation.sln
```

Playwright installs the Chromium browser on first run (via `PlaywrightSetup.cs`).

## Structure

- **Pages/** – Page Object Models (e.g. `LoginPage.cs`)
- **Tests/** – NUnit test classes (UI and API)
- **Utils/** – API helpers (e.g. `ApiHelper.cs`)
- **TestData/** – JSON/CSV test data (e.g. `testdata.json`)
