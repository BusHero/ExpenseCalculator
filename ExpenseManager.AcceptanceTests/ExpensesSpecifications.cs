namespace AcceptanceTests;

[Trait("Category", "Acceptance")]
public class ExpensesSpecifications : IClassFixture<RunConfiguration>
{
    private readonly IExpenses expenses;

    public ExpensesSpecifications(RunConfiguration runConfiguration)
    {
        expenses = runConfiguration.Expenses;
    }
    
    [Theory, AutoData]
    public async Task ShouldViewAddedExpenses(string expense, decimal amount)
    {
        await expenses.AddExpense(expense, amount);
        
        await expenses.AssertExpenseIsVisibleAsync(expense, amount);
    }
}