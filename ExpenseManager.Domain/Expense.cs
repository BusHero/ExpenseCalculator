namespace ExpenseManager.Domain;

public sealed record Expense
{
    public required ExpenseName Name { get; init; }
    
    public required Money Amount { get; init; }
}