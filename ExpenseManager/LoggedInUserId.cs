namespace ExpenseManager;

public record struct LoggedInUserId
{
    public required string Value { get; init; }
    
    public static LoggedInUserId FromString(string? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        
        return new LoggedInUserId()
        {
            Value = value,
        };
    }
}
