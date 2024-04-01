namespace ExpenseManager.Domain;

public sealed class User
{
    public UserId Id { get; private set; }

    public ExternalUserId ExternalId { get; private init; }

    public List<Expense> Expenses { get; private set; } = [];

    public void AddExpense(Expense expense)
    {
        Expenses.Add(expense);
    }

    public static User CreateNew(ExternalUserId id) 
        => new() { ExternalId = id, };
}