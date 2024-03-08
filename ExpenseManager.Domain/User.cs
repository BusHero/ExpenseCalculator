namespace ExpenseManager.Domain;

public class User
{
    public UserId Id { get; }
    
    public User(UserId id)
    {
        Id = id;
    }

    public void AddExpense(Expense expense)
    {
    }
}