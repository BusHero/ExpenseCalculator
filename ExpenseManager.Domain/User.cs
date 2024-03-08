namespace ExpenseManager.Domain;

public sealed class User
{
    public UserId Id { get; private set; }

    public List<Expense> Expenses { get; private set; } = [];

    private User() {}

    public User(UserId id)
    {
        Id = id;
    }

    public void AddExpense(Expense expense)
    {
        Expenses.Add(expense);
    }
}