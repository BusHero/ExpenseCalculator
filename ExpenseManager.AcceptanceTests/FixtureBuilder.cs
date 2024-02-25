using AutoFixture;

namespace AcceptanceTests;

public class FixtureBuilder
{
    private readonly IFixture fixture;
    private readonly HashSet<string> users = new HashSet<string>();
    private readonly Dictionary<string, HashSet<string>> expenses = new ();
    
    private readonly Func<IExpenses> expensesProvider;

    public FixtureBuilder(IFixture fixture, Func<IExpenses> expensesProvider)
    {
        this.fixture = fixture;
        this.expensesProvider = expensesProvider;
    }
    
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
