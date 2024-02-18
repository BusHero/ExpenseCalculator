namespace AcceptanceTests.Drivers;

public record ApiDriverOptions
{
    public const string Section = "ApiDriver";

    public required Uri Uri { get; init; }
}
