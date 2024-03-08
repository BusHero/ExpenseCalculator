using AutoFixture;
using AutoFixture.Xunit2;
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

    private readonly IFixture fixture = new Fixture();

    private Expense2 CreateExpense() => fixture.Create<Expense2>();

    [Fact]
    public void AddExpense()
    {
        using var context = CreateContext();
        var ogExpense = this.CreateExpense();
        context.Expenses.Add(ogExpense);

        context.SaveChanges();

        var expense = context.Expenses
            .Single(x => x.Id == ogExpense.Id);

        expense.Should().Be(ogExpense);
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
    public void AddUserWithExpense(
        ApplicationUser user,
        Expense2 expense)
    {
        using var context = CreateContext();
        user.Expenses = [ expense ];
        context.Users.Add(user);

        context.SaveChanges();

        var dbExpense = context.Expenses
            .First();

        dbExpense.Should().Be(dbExpense);
    }
}
