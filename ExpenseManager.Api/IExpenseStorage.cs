namespace ExpenseManager.Api;

public interface IExpenseStorage 
{
    void Add(Expense expense);
    
    IEnumerable<Expense> GetAll();
}