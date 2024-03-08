using AutoFixture.Xunit2;
using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DataAccess.Tests;

public class ApplicationContextTests
{
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

    [Theory, AutoData]
    public void AddUser(ApplicationUser user)
    {
        using var context = CreateContext();
        context.Users.Add(user);

        context.SaveChanges();

        var dbUser = context.Users
            .Single(x => x.Id == user.Id);

        dbUser.Should().Be(dbUser);
    }

    [Theory, AutoData]
    public void UserWithExpenses(
        User user,
        Expense expense
        )
    {
        user.AddExpense(expense);
        
        using var context = CreateContext();

        context.Users2.Add(user);
        context.SaveChanges();
    }
    
    [Theory, AutoData]
    public void ApplicationUserUserLink(
        ApplicationUser user1,
        User user2)
    {
        user1.User = user2;
        using var context = CreateContext();

        context.Users.Add(user1);
        
        context.SaveChanges();

        context.Users2
            .SingleOrDefault(x => x.Id == user2.Id)
            .Should()
            .NotBeNull();
    }

    [Fact]
    public void UserIdGetsAutoincrement()
    {
        var user1 = new User();
        var user2 = new User();

        var context = CreateContext();

        context.Users2.AddRange(user1, user2);

        context.SaveChanges();

        user1.Id.Should().NotBe(user2.Id);
    }
}