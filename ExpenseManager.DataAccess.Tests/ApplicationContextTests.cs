using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DataAccess.Tests;

public sealed class ApplicationContextTests
{
    private readonly IFixture fixture = new Fixture();
    
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

    [Fact]
    public void UserHasShadowPropertyEqualToApplicationId()
    {
        var context = CreateContext();

        var applicationUser = fixture.Create<ApplicationUser>();

        context.Users.Add(applicationUser);

        context.SaveChanges();

        context
            .Entry(applicationUser.User!)
            .Property("ApplicationUserId")
            .CurrentValue
            .Should()
            .Be(applicationUser.Id);
    }
    
    [Theory, AutoData]
    public void AddUser(ApplicationUser user)
    {
        using var context = CreateContext();
        context.Users.Add(user);

        context.SaveChanges();

        var savedUser = context.Users
            .Single(x => x.Id == user.Id);

        savedUser.Should().Be(savedUser);
    }

    [Theory, AutoData]
    public void UserWithExpenses(
        User user,
        Expense expense)
    {
        user.AddExpense(expense);
        
        using var context = CreateContext();

        context.DomainUsers.Add(user);
        context.SaveChanges();
    }
    
    [Theory, AutoData]
    public void ApplicationUserUserLink(
        ApplicationUser applicationUser,
        User domainUser)
    {
        applicationUser.User = domainUser;
        using var context = CreateContext();
    
        context.Users.Add(applicationUser);
        
        context.SaveChanges();
    
        context.DomainUsers
            .SingleOrDefault(x => x.Id == domainUser.Id)
            .Should()
            .NotBeNull();
    }

    [Fact]
    public void UserIdGetsAutoincrement()
    {
        var user1 = new User();
        var user2 = new User();

        var context = CreateContext();

        context.DomainUsers.AddRange(user1, user2);

        context.SaveChanges();

        user1.Id.Should().NotBe(user2.Id);
    }
}