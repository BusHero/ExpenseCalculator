using ExpenseManager.Domain;

namespace ExpenseManager.LocalDevelopment;

public class InMemoryExpensesStorage : IExpenseStorage 
{
    private readonly List<Expense> expenses = [];
    
    public void Add(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);
        
        expenses.Add(expense);
    }

    public IEnumerable<Expense> GetAll() 
        => expenses;
}