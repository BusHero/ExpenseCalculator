using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DataAccess.Tests;

public sealed class ApplicationContextTests
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
    public void UserWithExpenses(
        string id,
        Expense expense)
    {
        var user = User.CreateNew(ExternalUserId.FromString(id));
        user.AddExpense(expense);
        
        using var context = CreateContext();

        context.DomainUsers.Add(user);
        context.SaveChanges();
    }

    [Fact]
    public void UserIdGetsAutoincrement()
    {
        var user1 = User.CreateNew(ExternalUserId.FromString("foo"));
        var user2 = User.CreateNew(ExternalUserId.FromString("bar"));

        var context = CreateContext();

        context.DomainUsers.AddRange(user1, user2);

        context.SaveChanges();

        user1.Id.Should().NotBe(user2.Id);
    }
}