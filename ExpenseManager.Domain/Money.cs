namespace ExpenseManager.Domain;

public record struct Money
{
    public required decimal Value { get; init; }

    public static Money FromDecimal(decimal value) => new Money
    {
        Value = value,
    };
}
