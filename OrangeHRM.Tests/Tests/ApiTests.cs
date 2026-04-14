using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using OrangeHRM.Tests.Utils;
namespace OrangeHRM.Tests.Tests;

[AllureNUnit]
[TestFixture]
public class ApiTests
{
    [Test]
    [AllureSuite("API Tests")]
    [AllureSubSuite("Users Endpoint")]
    [AllureTag("API")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task GetUsers_ShouldReturn200AndValidResponseStructure()
    {
        var (response, users) = await ApiHelper.GetUsersAsync();

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
            "GET users should return status 200.");

        Assert.That(users, Is.Not.Empty, "Response should contain at least one user.");

        var first = users[0];
        Assert.That(first.TryGetProperty("id", out _), Is.True);
        Assert.That(first.TryGetProperty("name", out _), Is.True);
        Assert.That(first.TryGetProperty("username", out _), Is.True);
        Assert.That(first.TryGetProperty("email", out _), Is.True);
        Assert.That(first.TryGetProperty("address", out _), Is.True);
    }
}
