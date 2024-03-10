using AcceptanceTests.Drivers;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AcceptanceTests;

public class RunConfiguration : IAsyncLifetime
{
    private readonly ServiceProvider services;

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
    
    public FixtureBuilder NewBuilder()
    {
        return new FixtureBuilder(
            new Fixture(),
            () =>
            {
                var options = services.GetRequiredService<IOptions<DriverOptions>>();

                var service = services.GetRequiredKeyedService<IExpenses>(options.Value.Driver);

                return service;
            });
    }

    public Task InitializeAsync() => Task.CompletedTask;

    async Task IAsyncLifetime.DisposeAsync() => await services.DisposeAsync();
}