using NUnit.Framework;
using OrangeHRM.Tests.Utils;

namespace OrangeHRM.Tests.Tests;

[TestFixture]
public class ApiTests
{
    [Test]
    public async Task GetUsers_ShouldReturn200AndValidResponseStructure()
    {
        var (response, users) = await ApiHelper.GetUsersAsync();

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
            "GET users should return status 200.");
        Assert.That(users, Is.Not.Empty, "Response should contain at least one user.");

        var first = users[0];
        Assert.That(first.TryGetProperty("id", out _), Is.True, "User should have 'id'.");
        Assert.That(first.TryGetProperty("name", out _), Is.True, "User should have 'name'.");
        Assert.That(first.TryGetProperty("username", out _), Is.True, "User should have 'username'.");
        Assert.That(first.TryGetProperty("email", out _), Is.True, "User should have 'email'.");
        Assert.That(first.TryGetProperty("address", out _), Is.True, "User should have 'address'.");
    }
}
