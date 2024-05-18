using AutoFixture;

namespace AcceptanceTests;

public sealed class FixtureBuilder(
    IFixture fixture, 
    Func<IExpenses> expensesProvider)
{
    private readonly IFixture fixture = fixture;
    private readonly HashSet<string> users = new HashSet<string>();
    private readonly Dictionary<string, HashSet<string>> expenses = new ();

    public FixtureBuilder WithUser(string userId)
    {
        users.Add(userId);
        
        return this;
    }
    
    public FixtureBuilder WithExpense(string userId, string expenseId)
    {
        if (!this.expenses.TryGetValue(userId, out var expenses1))
        {
            expenses1 = new HashSet<string>();
        }

        expenses1.Add(expenseId);
        
        this.expenses[userId] = expenses1;
        
        return this;
    }
    
    public async Task<IExpenses> BuildAsync()
    {
        var driver = expensesProvider();

        await driver.InitializeAsync();

        foreach (var userId in users)
        {
            await driver.RegisterUserAsync(userId);
        }

        foreach (var (userId, expenseIds) in this.expenses)
        {
            foreach (var expenseId in expenseIds)
            {
                await driver.AddExpense(userId, expenseId);
            }
        }

        return driver;
    }
}
