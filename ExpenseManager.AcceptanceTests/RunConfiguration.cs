using AcceptanceTests.Drivers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcceptanceTests;

public class RunConfiguration
{
    public Expenses Expenses { get; }
    
    public RunConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .Build();

        var collection = new ServiceCollection();

        var services = collection
            .Configure<DriverOptions>(configuration.GetSection(DriverOptions.Section))
            .Configure<ApiDriverOptions>(configuration.GetSection(ApiDriverOptions.Section))
            .AddKeyedScoped<IDriver, ApiDriver>(DriverOptions.Api)
            .AddKeyedScoped<IDriver, WebDriver>(DriverOptions.Web)
            .AddScoped<DriverProvider>()
            .AddScoped<Expenses>()
            .BuildServiceProvider();
        
        Expenses = services.GetRequiredService<Expenses>();
    }
}
