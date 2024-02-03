﻿namespace AcceptanceTests;

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