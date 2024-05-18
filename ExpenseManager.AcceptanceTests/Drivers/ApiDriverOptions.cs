namespace AcceptanceTests.Drivers;

public sealed record ApiDriverOptions
{
    public const string Section = "ApiDriver";

    public required Uri Uri { get; init; }
}
