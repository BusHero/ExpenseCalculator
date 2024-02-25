﻿using System.Net.Http.Json;
using AutoFixture;

namespace AcceptanceTests.Drivers;

public class ApiDriver : IExpenses
{
    private readonly HttpClient client;
    private Dictionary<string, Expense> expenses = new Dictionary<string, Expense>();
    private readonly Fixture fixture;

    public ApiDriver(HttpClient client)
    {
        this.client = client;
        this.fixture = new Fixture();
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

    public async Task AddExpense(string userId, string expenseId)
    {
        var name = fixture.Create<string>();
        var amount = fixture.Create<decimal>();

        this.expenses[expenseId] = new Expense(name, amount);

        await PerformAddExpense(name, amount);
    }
    
    private async Task PerformAddExpense(string name, decimal amount)
    {
        await client.PostAsJsonAsync(
            "/expense/create", 
            new Expense(name, amount));
    }

    public async Task AssertExpenseIsVisibleAsync(
        string userId, 
        string expenseId)
    {
        var expense = expenses[expenseId];
        
        var result = await client
            .GetFromJsonAsync<List<Expense>>("/expense/all");

        result.Should()
            .ContainEquivalentOf(expense);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    
    public Task RegisterUserAsync(string userId) 
        => Task.CompletedTask;
    
    public async Task AssertExpenseIsNotVisibleAsync(string userId, string expenseId)
    {
        var expense = expenses[expenseId];
        
        var result = await client
            .GetFromJsonAsync<List<Expense>>("/expense/all");

        result.Should()
            .NotContainEquivalentOf(expense);
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}