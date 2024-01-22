namespace AcceptanceTests;

public class ExpensesSpecifications 
{
    private readonly Expenses expenses = new Expenses();

    [Theory, AutoData]
    public async Task ShouldViewAddedExpenses(string expense, decimal amount)
    {
        await expenses.AddExpense(expense, amount);
        
        expenses.NavigateExpensesPage();
        
        await expenses.AssertExpensesAreVisible(expense, amount);
    }
}