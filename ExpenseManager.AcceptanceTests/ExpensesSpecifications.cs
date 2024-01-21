using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace TestProject2;

public class ExpensesSpecifications 
{
    private readonly Expenses expenses = new Expenses();

    [Fact] 
    public async Task ShouldViewAddedExpenses()
    {
        await expenses.AddExpense("Food", 123.12m);
        
        expenses.NavigateExpensesPage();
        
        await expenses.AssertExpensesAreVisible("Food", 123.12m);
    }
}

public class Expenses 
{
    private readonly ApiDriver driver = new ApiDriver();

    public async Task AddExpense(string food, decimal amount)
    {
        await driver.AddExpense(new Expense(food, amount));
    }
    public async Task<List<Expense>?> ViewExpenses()
    {
        var expenses = await driver.GetExpenses();

        return expenses;
    }
    
    public async Task AssertExpensesAreVisible(string name, decimal amount)
    {
        var expenses = await driver.GetExpenses();

        expenses.Should().ContainEquivalentOf(new Expense(name, amount));
    }
    
    public void NavigateExpensesPage()
    {
        driver.NavigateExpensesPage();
    }
}

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

public record Expense(string Name, decimal Amount);