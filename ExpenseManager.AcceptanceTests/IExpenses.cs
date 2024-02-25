namespace AcceptanceTests;

public interface IExpenses 
{
    Task AddExpense(string userId, string expenseId);
    
    Task AssertExpenseIsVisibleAsync(string userId, string expenseId);
    
    Task InitializeAsync();
    
    Task RegisterUserAsync(string userId);
    
    Task AssertExpenseIsNotVisibleAsync(string userId, string expenseId);
}
