using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IdentityServer.Tests;

public sealed class UnitTest1 :
    IClassFixture<ApiApplicationFactory>
{
    private readonly HttpClient identityServerClient;
    private readonly HttpClient apiClient;

    public UnitTest1(
        ApiApplicationFactory apiFixture)
    {

        identityServerClient = new HttpClient();
        identityServerClient.BaseAddress = new("https://localhost:5001");
        
        apiClient = apiFixture.CreateClient(new ()
        {
            BaseAddress = new("https://localhost:6001"),
        });
    }

    [Fact]
    public async Task Test1()
    {
        var disco = await identityServerClient
            .GetDiscoveryDocumentAsync();

        var tokenResponse = await identityServerClient.RequestClientCredentialsTokenAsync(new()
        {
            Address = disco.TokenEndpoint,
            ClientId = "client",
            ClientSecret = "secret",
            Scope = "api1",
        });

        apiClient.SetBearerToken(tokenResponse.AccessToken!);

        var response = await apiClient
            .GetAsync("identity");

        response.EnsureSuccessStatusCode();
    }
}

public sealed class IdentityServerApplicationFactory
    : WebApplicationFactory<IdentityServer.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("https_port", "5001");// Specify the desired HTTPS port
        builder.UseEnvironment("Development");// Optional: Set the environment to Development

        base.ConfigureWebHost(builder);
    }
}

public sealed class ApiApplicationFactory
    : WebApplicationFactory<Api.Program>;
