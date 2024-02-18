namespace AcceptanceTests.Drivers;

public class DriverOptions
{
    public const string Section = "Drivers";
    
    public const string Web = "Web";
    public const string Api = "Api";

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string Driver { get; set; } = null!;
}
