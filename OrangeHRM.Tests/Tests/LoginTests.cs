using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using NUnit.Framework;
using OrangeHRM.Tests.Pages;
using static Microsoft.Playwright.Assertions;
using NUnit.Framework.Interfaces;

namespace OrangeHRM.Tests.Tests;

[AllureNUnit]
[TestFixture]
public class LoginTests
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    private const string BaseUrl = "https://opensource-demo.orangehrmlive.com/";

    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false // Set to true for Jenkins/CI, false for local debugging
        });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        // 1. Capture test outcome
        var outcome = TestContext.CurrentContext.Result.Outcome.Status;
        var testName = TestContext.CurrentContext.Test.Name;

        if (_page != null)
        {
            try
            {
                // 2. Screenshot name
                string fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmm}.png";

                // 3. Take screenshot BEFORE cleanup
                byte[] screenshot = await _page.ScreenshotAsync(new()
                {
                    FullPage = true
                });

                // 4. Attach to Allure (safe modern API)
                AllureApi.AddAttachment(
                    fileName,
                    "image/png",
                    screenshot
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Screenshot failed: {ex.Message}");
            }
        }

        // 5. Cleanup (IMPORTANT ORDER FIX)
        try
        {
            if (_context != null)
            {
                await _context.ClearCookiesAsync();
                await _context.CloseAsync();
                _context = null;
            }

            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }

            _playwright?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cleanup failed: {ex.Message}");
        }
    }

    [Test]
    [AllureSuite("UI Tests")]
    [AllureSubSuite("Login")]
    [AllureTag("UI")]
    public async Task Login_WithValidCredentials_ShouldShowDashboard()
    {
        Assert.That(_page, Is.Not.Null);

        var loginPage = new LoginPage(_page!, BaseUrl.TrimEnd('/'));

        await loginPage.GotoAsync();
        await loginPage.LoginAsync("Admin", "admin123");

        var dashboardHeading = _page!.GetByRole(
            AriaRole.Heading,
            new() { Name = "Dashboard" }
        );

        await Expect(dashboardHeading)
            .ToBeVisibleAsync(new() { Timeout = 10000 });
    }
}