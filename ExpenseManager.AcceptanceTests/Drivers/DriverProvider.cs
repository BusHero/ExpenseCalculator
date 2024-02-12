using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AcceptanceTests.Drivers;

public class DriverProvider
{
    private readonly IServiceProvider serviceProvider;
    private readonly string? driver;

    public DriverProvider(
        IServiceProvider serviceProvider,
        IOptions<DriverOptions> driversOptions)
    {
        this.serviceProvider = serviceProvider;
        driver = driversOptions.Value.Driver;
    }
    
    public IDriver GetDriver() 
        => serviceProvider.GetRequiredKeyedService<IDriver>(driver);
}

public class DriverOptions
{
    public const string Section = "Drivers";
    
    public const string Web = "Web";
    public const string Api = "Api";

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string Driver { get; set; } = null!;
}