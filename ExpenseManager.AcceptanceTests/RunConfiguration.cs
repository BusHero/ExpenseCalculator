using AcceptanceTests.Drivers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AcceptanceTests;

public class RunConfiguration : IAsyncLifetime
{
    private readonly ServiceProvider services;
    public IExpenses Expenses { get; private set; } = null!;
    
    public RunConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var collection = new ServiceCollection();

        services = collection
            .Configure<DriverOptions>(configuration.GetSection(DriverOptions.Section))
            .Configure<WebDriverOptions>(configuration.GetSection(WebDriverOptions.Section))
            .Configure<ApiDriverOptions>(configuration.GetSection(ApiDriverOptions.Section))
            .AddKeyedScoped<IExpenses, ApiDriver>(DriverOptions.Api)
            .AddKeyedScoped<IExpenses, WebDriver>(DriverOptions.Web)
            .BuildServiceProvider();
    }
    
    public async Task InitializeAsync()
    {
        var options = services.GetRequiredService<IOptions<DriverOptions>>();
        
        Expenses = services.GetRequiredKeyedService<IExpenses>(options.Value.Driver);
        
        await Expenses.InitializeAsync();
    }

    public async Task DisposeAsync() 
        => await services.DisposeAsync();
}
