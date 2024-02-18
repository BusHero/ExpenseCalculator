using System.Net.Http.Json;

namespace AcceptanceTests.Drivers;

public class ApiDriver : IExpenses
{
    private readonly HttpClient client;
    
    public ApiDriver(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task AddExpense(Expense expense)
    {
        await client.PostAsJsonAsync(
            "/expense/create", 
            expense);
    }
    
    public async Task<List<Expense>?> GetExpenses()
    {
        var result = await client
            .GetFromJsonAsync<List<Expense>>("/expense/all");

        return result;
    }
    
    public async Task AddExpense(string name, decimal amount)
    {
        await client.PostAsJsonAsync(
            "/expense/create", 
            new Expense(name, amount));
    }

    public async Task AssertExpenseIsVisibleAsync(string name, decimal amount)
    {
        var result = await client
            .GetFromJsonAsync<List<Expense>>("/expense/all");

        result.Should()
            .ContainEquivalentOf(new Expense(name, amount));
    }

    public Task InitializeAsync() => Task.CompletedTask;
    
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}