using ExpenseManager.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseManager.IntegrationTests;

public class MyWebFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureTestServices(services =>
            {
                services.RemoveAll<IApplicationService>();
                services.AddTransient<IApplicationService, FakeApplicationService>();
                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme",
                        options => {});
            })
            .ConfigureServices(x =>
            {
                x.AddAntiforgery(t =>
                {
                    t.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                    t.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
                });
            });
    }
    public void WithUserAuthenticationStatus(bool authStatus)
    {
        TestAuthHandler.SetAuthStatus(authStatus);
    }
}

public class FakeApplicationService : IApplicationService
{
    public void AddExpenseToLoggedInUser(LoggedInUserId loggedInUserId, Expense expense)
    {
    }
    
    public IEnumerable<Expense> GetExpensesLoggedInUser(LoggedInUserId loggedInUserId) 
        => [];
}