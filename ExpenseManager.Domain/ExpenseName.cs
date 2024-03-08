namespace ExpenseManager.Domain;

public record struct ExpenseName
{
    public string Value { get; private set; }

    public static ExpenseName FromString(string name) => new ExpenseName
    {
        Value = name,
    };
}
