namespace AcceptanceTests;

public interface IExpenses : IAsyncDisposable
{
    Task AddExpense(string name, decimal amount);
    
    Task AssertExpenseIsVisibleAsync(string name, decimal amount);
    
    Task InitializeAsync();
}
