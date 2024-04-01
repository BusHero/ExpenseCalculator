namespace ExpenseManager.Domain;

public record struct ExternalUserId
{
    public required string Value { get; init; }
    
    public static ExternalUserId FromString(string? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        
        return new()
        {
            Value = value,
        };
    }
}
