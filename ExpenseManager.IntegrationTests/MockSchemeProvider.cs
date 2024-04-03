using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ExpenseManager.IntegrationTests;

public class MockSchemeProvider : AuthenticationSchemeProvider
{
    public MockSchemeProvider(IOptions<AuthenticationOptions> options)
        : base(options)
    {
    }

    protected MockSchemeProvider(
        IOptions<AuthenticationOptions> options,
        IDictionary<string, AuthenticationScheme> schemes
    )
        : base(options, schemes)
    {
    }

    public override Task<AuthenticationScheme?> GetSchemeAsync(string name)
    {
        var scheme = new AuthenticationScheme(
            "Test",
            "Test",
            typeof(TestAuthHandler)
        );
        return Task.FromResult(scheme) as Task<AuthenticationScheme?>;

    }
}
