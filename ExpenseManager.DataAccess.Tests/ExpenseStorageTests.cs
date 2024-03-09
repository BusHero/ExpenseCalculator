using AutoFixture.Xunit2;
using ExpenseManager.Domain;
using ExpenseManager.LocalDevelopment;
using FluentAssertions;

namespace ExpenseManager.DataAccess.Tests;

public class ExpenseStorageTests
{
    private readonly IExpenseStorage storage;
    
    public ExpenseStorageTests()
    {
        storage = new InMemoryExpensesStorage();
    }
    
    [Theory, AutoData]
    public void ExistingUserReturnsUser(User user)
    {
        storage.Save(user);

        var newUser = storage.GetUser(user.Id);

        newUser!.Id.Should().Be(user.Id);
    }
    
    [Theory, AutoData]
    public void NonExistingUserReturnsNull(User user, UserId newUserId)
    {
        storage.Save(user);

        var newUser = storage.GetUser(newUserId);

        newUser.Should().BeNull();
    }
}
