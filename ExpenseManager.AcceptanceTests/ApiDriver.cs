using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace AcceptanceTests;

public class ApiDriver
{
    private readonly HttpClient client;
    
    public ApiDriver()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .Build();

        var uri = configuration["uri"];
        
        client = new HttpClient();
        client.BaseAddress = new Uri(uri!);
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
    
    public void NavigateExpensesPage()
    {
    }
}
