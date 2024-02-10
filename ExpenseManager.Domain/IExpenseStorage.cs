namespace ExpenseManager.Domain;

public interface IExpenseStorage 
{
    void Add(Expense expense);
    
    IEnumerable<Expense> GetAll();
}