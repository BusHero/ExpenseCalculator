namespace AcceptanceTests;

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