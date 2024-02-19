using AcceptanceTests.Drivers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AcceptanceTests;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiDriver(
        this IServiceCollection services,
        string key) 
        => services
            .AddHttpClientApiDriver(key)
            .AddApiDriverService(key);

    private static IServiceCollection AddHttpClientApiDriver(
        this IServiceCollection services,
        string name) 
        => services
            .AddHttpClient(
                name,
                (provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptions<ApiDriverOptions>>();

                    client.BaseAddress = options.Value.Uri;
                })
            .Services;

    private static IServiceCollection AddApiDriverService(
        this IServiceCollection services, 
        string key)
        => services
            .AddKeyedTransient<IExpenses, ApiDriver>(
                key,
                (provider, _) =>
                {
                    var factory = provider.GetRequiredService<IHttpClientFactory>();

                    var client = factory.CreateClient(DriverOptions.Api);

                    return new ApiDriver(client);
                });
}
