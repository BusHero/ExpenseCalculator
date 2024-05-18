using ExpenseManager.Domain;

namespace ExpenseManager.LocalDevelopment;

public sealed class InMemoryExpensesStorage : IExpenseStorage 
{
    private readonly List<Expense> expenses = [];
    
    public void Add(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);
        
        expenses.Add(expense);
    }

    public IEnumerable<Expense> GetAll() 
        => expenses;

    private readonly List<User> users = [];
    
    public void Save(User user)
    {
        users.Add(user);
    }
    
    public User? GetUser(UserId userId)
    {
        return users.FirstOrDefault(x => x.Id == userId);
    }
}