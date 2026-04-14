using Microsoft.Playwright;

namespace OrangeHRM.Tests.Pages;

public class LoginPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    private ILocator UsernameInput => _page.Locator("input[name=\"username\"]");
    private ILocator PasswordInput => _page.Locator("input[name=\"password\"]");
    private ILocator LoginButton => _page.Locator("button[type=\"submit\"]");

    public LoginPage(IPage page, string baseUrl = "https://opensource-demo.orangehrmlive.com")
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync(_baseUrl);
    }

    public async Task LoginAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }
}
