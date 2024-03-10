using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.IntegrationTests;

public class ApplicationServiceTests
{
    private readonly ApplicationServiceFixtureBuilder builder = new();
    
    [Theory, AutoData]
    public void AddExpenseToLoggedInUser_AddExpenseToUser(Expense expense)
    {
        using var fixture = builder
            .Build()
            .WithApplication(out var applicationId);

        fixture.ApplicationService
            .AddExpenseToLoggedInUser(
                LoggedInUserId.FromString(applicationId),
                expense);

        fixture.AssertDomainUserHasExpense(applicationId, expense);
    }
    
    [Theory, AutoData]
    public void AddExpenseToLoggedInUser_NonExistingDomainUserCreatesUser(Expense expense)
    {
        using var fixture = builder
            .Build()
            .WithApplication(
                x => x.User = null,
                out var applicationId);

        fixture.ApplicationService
            .AddExpenseToLoggedInUser(
                LoggedInUserId.FromString(applicationId),
                expense);

        fixture.AssertDomainUserHasExpense(applicationId, expense);
    }
    
    [Fact]
    public void GetExpensesLoggedInUser_NonExistingUser_ReturnsEmptyList()
    {
        using var fixture = builder
            .Build()
            .WithApplication(
                x => x.User = null,
                out var applicationId);

        var expenses = fixture.ApplicationService
            .GetExpensesLoggedInUser(
                LoggedInUserId.FromString(applicationId));

        expenses
            .Should()
            .BeEmpty();
    }
    
    [Theory, AutoData]
    public void GetExpensesLoggedInUser_ReturnsExpenses(Expense expense)
    {
        using var fixture = builder
            .Build()
            .WithApplication(
                x =>
                {
                    var user = new User();

                    x.User = user;
                    
                    user.AddExpense(expense);
                },
                out var applicationId);

        var expenses = fixture.ApplicationService
            .GetExpensesLoggedInUser(
                LoggedInUserId.FromString(applicationId));

        expenses
            .Should()
            .Contain(expense)
            .And
            .Subject
            .Should()
            .ContainSingle();
    }
}


public class ApplicationServiceFixtureBuilder
{
    public ApplicationServiceFixture Build()
    {
        var fixture = new ApplicationServiceFixture();
        fixture.Initialize();
        return fixture;
    }
}

public class ApplicationServiceFixture : IDisposable, IAsyncDisposable
{
    private readonly IFixture fixture = new Fixture();

    private ApplicationContext Context { get; set; } = null!;

    public ApplicationService ApplicationService { get; private set; } = null!;

    public void Initialize()
    {
        Context = CreateContext();
        ApplicationService = new ApplicationService(Context);
    }
    
    public void Dispose() => Context.Dispose();
    
    public async ValueTask DisposeAsync() => await Context.DisposeAsync();
    
    private static DbContextOptions<ApplicationContext> GetSqLiteInMemoryOptions()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseSqlite(connection)
            .Options;

        return options;
    }

    private static ApplicationContext CreateContext()
    {
        var options = GetSqLiteInMemoryOptions();
        var context = new ApplicationContext(options);
        context.Database.EnsureCreated();

        return context;
    }

    public ApplicationServiceFixture WithApplication(out string applicationId)
    {
        return WithApplication(_ => {}, out applicationId);
    }

    public ApplicationServiceFixture WithApplication(
        Action<ApplicationUser> config,
        out string applicationId)
    {
        var user = fixture.Create<ApplicationUser>();
        config(user);
        
        Context.Users.Add(user);
        Context.SaveChanges();

        applicationId = user.Id;
        
        return this;
    }
    
    public void AssertDomainUserHasExpense(string applicationId, Expense expense)
    {
        Context
            .Users
            .Include(user => user.User!)
            .ThenInclude(user => user.Expenses)
            .Single(x => x.Id == applicationId)
            .User!
            .Expenses
            .Should()
            .Contain(expense);
    }
}
