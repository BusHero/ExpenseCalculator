using Microsoft.AspNetCore.Mvc.Testing;

namespace ExpenseManager.IntegrationTests;

[Trait("Category", "Integration")]
public class IndexTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;
    
    public IndexTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ShouldReturn200()
    {
        var response = await client
            .GetAsync("/");

        response.EnsureSuccessStatusCode();
    }
}
