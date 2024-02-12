using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace AcceptanceTests.Drivers;

public class ApiDriverOptions
{
    public const string Section = "ApiDriver";

 #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Uri Uri { get; set; }
 #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

public class ApiDriver : IDriver
{
    private readonly HttpClient client;
    
    public ApiDriver(IOptions<ApiDriverOptions> apiDriverOptions)
    {
        var uri = apiDriverOptions.Value.Uri;
        
        client = new HttpClient();
        
        client.BaseAddress = uri;
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