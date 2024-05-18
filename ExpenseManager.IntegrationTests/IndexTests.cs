using Microsoft.AspNetCore.Mvc.Testing;

namespace ExpenseManager.IntegrationTests;

[Trait("Category", "Integration")]
public sealed class IndexTests(WebApplicationFactory<Program> factory) 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task Get_ShouldReturn200()
    {
        var response = await client
            .GetAsync("/");

        response.EnsureSuccessStatusCode();
    }
}
