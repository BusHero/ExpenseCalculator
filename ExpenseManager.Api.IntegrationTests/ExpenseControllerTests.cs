using System.Net.Http.Json;
using ExpenseManager.Domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExpenseManager.Api.IntegrationTests;

[Trait("Category", "Integration")]
public sealed class ExpenseControllerTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client ;

    public ExpenseControllerTests(
        WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Theory, AutoData] 
    public async Task WhenPostingExpense_ShouldReturn200(
        Expense expense)
    {
        var response = await client.PostAsJsonAsync(
            "/expense/create", 
            expense);

        response.EnsureSuccessStatusCode();
    }
}