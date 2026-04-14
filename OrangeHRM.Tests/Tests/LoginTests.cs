using Microsoft.Playwright;
using NUnit.Framework;
using OrangeHRM.Tests.Pages;
using static Microsoft.Playwright.Assertions;

namespace OrangeHRM.Tests.Tests;

[TestFixture]
public class LoginTests
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _page;
    private const string BaseUrl = "https://opensource-demo.orangehrmlive.com/";

    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        _page = await _browser.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_page != null) await _page.CloseAsync();
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }

    [Test]
    public async Task Login_WithValidCredentials_ShouldShowDashboard()
    {
        Assert.That(_page, Is.Not.Null);
        var loginPage = new LoginPage(_page!, BaseUrl.TrimEnd('/'));
        await loginPage.GotoAsync();
        await loginPage.LoginAsync("Admin", "admin123");

        var dashboardHeading = _page!.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" });
        await Expect(dashboardHeading).ToBeVisibleAsync(new() { Timeout = 10000 });
    }
}
