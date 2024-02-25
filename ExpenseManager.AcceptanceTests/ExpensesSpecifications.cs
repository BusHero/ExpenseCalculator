namespace AcceptanceTests;

[Trait("Category", "Acceptance")]
public class ExpensesSpecifications : IClassFixture<RunConfiguration>
{
    private readonly RunConfiguration runConfiguration;

    public ExpensesSpecifications(RunConfiguration runConfiguration)
    {
        this.runConfiguration = runConfiguration;
    }
    
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