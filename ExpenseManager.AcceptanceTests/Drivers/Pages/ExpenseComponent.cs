using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public class ExpenseComponent
{
    private readonly ILocator baseLocator;
    public ExpenseComponent(ILocator baseLocator)
    {
        this.baseLocator = baseLocator;
    }

    private ILocator Name => baseLocator.Locator(".name");

    private ILocator Amount => baseLocator.Locator(".amount");

    public async Task<Expense> GetExpense()
    {
        var name = await Name.InnerTextAsync();
        var amount = await Amount.InnerTextAsync();

        return new Expense(
            name, 
            decimal.Parse(amount));
    }
}
