namespace AcceptanceTests;

[Trait("Category", "Acceptance")]
public sealed class ExpensesSpecifications(
    RunConfiguration runConfiguration) : IClassFixture<RunConfiguration>
{
    [Fact]
    public async Task UserCanSeeHisExpenses()
    {
        var fixture = await runConfiguration
            .NewBuilder()
            .WithUser("John")
            .WithUser("Andrew")
            .WithExpense("John", "expense")
            .BuildAsync();

        await fixture.AssertExpenseIsVisibleAsync("John", "expense");
    }
    
    [Fact]
    public async Task UserCannotSeeExpensesOfSomeoneElse()
    {
        var fixture = await runConfiguration
            .NewBuilder()
            .WithUser("John")
            .WithUser("Andrew")
            .WithExpense("John", "expense")
            .BuildAsync();

        await fixture.AssertExpenseIsNotVisibleAsync("Andrew", "expense");
    }
}