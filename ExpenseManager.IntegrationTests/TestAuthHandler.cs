using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExpenseManager.IntegrationTests;

public sealed class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private static bool _authStatus;

    public static void SetAuthStatus(bool authStatus2)
    {
        _authStatus = authStatus2;
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!_authStatus)
        {
            return AuthenticateResult.Fail(new Exception());
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("sub", "123456789"),
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");
        var result = AuthenticateResult.Success(ticket);

        return await Task.FromResult(result);
    }
}