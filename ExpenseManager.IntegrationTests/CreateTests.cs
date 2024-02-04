using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExpenseManager.IntegrationTests;

public class CreateTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;
    
    public CreateTests(
        WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }
    
    [Fact]
    public async Task Get_Returns200()
    {
        var status = await client.GetAsync("/Create");

        status.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Post_ReturnRedirect()
    {
        var content = new FormUrlEncodedContent([
            new("Expense", "bar"),
            new("Amount", "123.123"),
        ]);

        var response = await client.PostAsync(
            "/Create",
            content);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Redirect);
    }
    
    [Fact]
    public async Task Post_RedirectToIndex()
    {
        var content = new FormUrlEncodedContent([
            new("Expense", "bar"),
            new("Amount", "123.123"),
        ]);

        var response = await client.PostAsync(
            "/Create",
            content);

        response
            .Headers
            .Location!
            .OriginalString
            .Should()
            .Be("/");
    }
}
