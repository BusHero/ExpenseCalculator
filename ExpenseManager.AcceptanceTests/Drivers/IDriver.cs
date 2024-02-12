namespace AcceptanceTests.Drivers;

public interface IDriver
{
    Task AddExpense(Expense expense);
    
    Task<List<Expense>?> GetExpenses();
    
    void NavigateExpensesPage();
}