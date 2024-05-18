namespace AcceptanceTests.Drivers;

public sealed record WebDriverOptions
{
    public const string Section = "WebDriver";

    public required Uri Uri { get; init; }
    
    public required bool Headless { get; init; }
}
