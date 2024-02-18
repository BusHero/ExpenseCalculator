namespace AcceptanceTests.Drivers;

public record WebDriverOptions
{
    public const string Section = "WebDriver";

    public required Uri Uri { get; init; }
    
    public required bool Headless { get; init; }
}
