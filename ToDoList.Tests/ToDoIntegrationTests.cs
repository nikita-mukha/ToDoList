using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Infrastructure.Identity;
using ToDoList.Web.Models;

namespace ToDoList.Tests;

public class ToDoIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ToDoIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetEvents_WhenUserIsNotAuthenticated_RedirectsToLogin()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/ToDo/Events");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Contains("/Auth/Login", response.Headers.Location!.OriginalString);
    }

    [Fact]
    public async Task Login_WithReturnUrl_RedirectsToRequestedPage()
    {
        var userName = $"testuser{Guid.NewGuid()}";
        await CreateUserAsync(userName, "qwerty12");

        var client = CreateClient();

        var formData = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("UserName", userName),
            new("Password", "qwerty12"),
            new("returnUrl", "/ToDo/Events")
        });

        var response = await client.PostAsync("/Auth/Login", formData);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Equal("/ToDo/Events", response.Headers.Location!.OriginalString);
    }

    [Fact]
    public async Task GetEvents_AfterLogin_ReturnsEventPage()
    {
        var userName = $"testuser{Guid.NewGuid()}";
        await CreateUserAsync(userName, "qwerty12");

        var client = CreateClient();

        await LoginAsync(client, userName, "qwerty12");

        var response = await client.GetAsync("/ToDo/Events");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Event Log", content);
    }

    [Fact]
    public async Task CreateTodo_AfterLogin_AddsEventToEventLog()
    {
        var userName = $"testuser{Guid.NewGuid()}";
        await CreateUserAsync(userName, "qwerty12");

        var client = CreateClient();

        await LoginAsync(client, userName, "qwerty12");

        var formCreateCall = CreateTaskFormData("TestTitle", "TestDescription");

        await client.PostAsync("/ToDo/Create", formCreateCall);
        var response = await client.GetAsync("/ToDo/Events");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("TestTitle", content);
        Assert.Contains("Added", content);
    }

    private static FormUrlEncodedContent CreateTaskFormData(
        string title,
        string description,
        DateTime? targetDayTime = null)
    {
        var date = targetDayTime ?? DateTime.Today;

        return new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("Title", title),
            new("Description", description),
            new("TargetDayTime", date.ToString("yyyy-MM-ddTHH:mm")),
            new("ItemType", "4")
        });
    }

    private async Task CreateUserAsync(string userName, string password)
    {
        using var scope = _factory.Services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser
        {
            UserName = userName
        };

        var result = await userManager.CreateAsync(user, password);

        Assert.True(result.Succeeded);
    }

    private async Task LoginAsync(HttpClient client, string userName, string password)
    {
        var formData = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("UserName", userName),
            new("Password", password)
        });

        var response = await client.PostAsync("/Auth/Login", formData);
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    private HttpClient CreateClient()
    {
        return _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
}