namespace ExpenseManager.Domain;

public sealed class User
{
    public UserId Id { get; private set; }

    public List<Expense> Expenses { get; private set; } = [];

    public void AddExpense(Expense expense)
    {
        Expenses.Add(expense);
    }
}