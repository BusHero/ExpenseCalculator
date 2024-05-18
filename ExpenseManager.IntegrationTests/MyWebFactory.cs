using System.Data.Common;
using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseManager.IntegrationTests;

public sealed class MyWebFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationContext>));

            services.Remove(dbContextDescriptor!);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbConnection));

            services.Remove(dbConnectionDescriptor!);
            
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<ApplicationContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });
        builder
            .ConfigureTestServices(services =>
            {
                services.RemoveAll<IApplicationService>();
                services.AddTransient<IApplicationService, FakeApplicationService>();
                services.AddAuthentication(defaultScheme: "Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme",
                        options => {});
                services.AddTransient<IAuthenticationSchemeProvider, MockSchemeProvider>();
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

public sealed class FakeApplicationService : IApplicationService
{
    public void AddExpenseToLoggedInUser(ExternalUserId externalUserId, Expense expense)
    {
    }
    
    public IEnumerable<Expense> GetExpensesLoggedInUser(ExternalUserId externalUserId) 
        => [];
    
    public Task CreateNewUser(ExternalUserId id) => Task.CompletedTask;
}