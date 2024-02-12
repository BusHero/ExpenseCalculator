namespace AcceptanceTests.Drivers;

public class WebDriver : IDriver
{
    public Task AddExpense(Expense expense) => throw new NotImplementedException();
    public Task<List<Expense>?> GetExpenses() => throw new NotImplementedException();
    public void NavigateExpensesPage()
    {
        throw new NotImplementedException();
    }
}
