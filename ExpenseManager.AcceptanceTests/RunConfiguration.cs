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

        services = new ServiceCollection()
            .Configure<DriverOptions>(configuration.GetSection(DriverOptions.Section))
            .Configure<WebDriverOptions>(configuration.GetSection(WebDriverOptions.Section))
            .Configure<ApiDriverOptions>(configuration.GetSection(ApiDriverOptions.Section))
            .AddApiDriver(DriverOptions.Api)
            .AddKeyedTransient<IExpenses, WebDriver>(DriverOptions.Web)
            .BuildServiceProvider(true);
    }

    public async Task InitializeAsync()
    {
        var options = services.GetRequiredService<IOptions<DriverOptions>>();

        Expenses = services.GetRequiredKeyedService<IExpenses>(options.Value.Driver);

        await Expenses.InitializeAsync();
    }

    public async Task DisposeAsync()
        => await services.DisposeAsync();
    
    public FixtureBuilder NewBuilder()
    {
        return new FixtureBuilder();
    }
}

public class FixtureBuilder
{
    public FixtureBuilder WithUser(string userId)
    {
        return this;
    }
    
    public FixtureBuilder WithExpense(string userId, string expense, decimal amount)
    {
        return this;
    }
    
    public Task<ProcessFixture> BuildAsync()
    {
        return Task.FromResult(new ProcessFixture());
    }
}

public class ProcessFixture
{
    public void AssertExpenseIsVisibleAsync(
        string userId, 
        string expense, 
        decimal amount)
    {
    }
}