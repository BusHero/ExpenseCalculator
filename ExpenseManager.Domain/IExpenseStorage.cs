namespace ExpenseManager.Domain;

public interface IExpenseStorage 
{
    void Add(Expense expense);
    
    IEnumerable<Expense> GetAll();
    
    void Save(User user);
    
    User? GetUser(UserId userId);
}