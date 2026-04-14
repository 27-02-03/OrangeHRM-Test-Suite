using NUnit.Framework;

namespace OrangeHRM.Tests;

/// <summary>
/// One-time setup: ensure Playwright browsers are installed so tests can run out-of-the-box.
/// </summary>
[SetUpFixture]
public static class PlaywrightSetup
{
    [OneTimeSetUp]
    public static void InstallPlaywrightBrowsers()
    {
        var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });
        if (exitCode != 0)
            Assert.Warn($"Playwright browser install exited with code {exitCode}. Run: pwsh bin/Debug/net10.0/playwright.ps1 install chromium");
    }
}
