using System.Net.Http.Json;
using AutoFixture.Xunit2;
using ExpenseManager.Domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExpenseManager.Api.IntegrationTests;

public class ExpenseControllerTests 
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

    [Theory, AutoData] 
    public async Task WhenGettingExpense_ShouldGetWhatWasPostedBefore(
        Expense expense)
    {
        await client.PostAsJsonAsync(
            "/expense/create", 
            expense);

        var expenses = await client
            .GetFromJsonAsync<List<Expense>>("/expense/all");

        expenses.Should().Contain(expense);
    }
}