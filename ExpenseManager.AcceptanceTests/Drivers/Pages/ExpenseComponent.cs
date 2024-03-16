using System.Globalization;
using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public class ExpenseComponent : PageBase
{
    public ExpenseComponent(ILocator baseLocator) : base(baseLocator)
    { }

    private ILocator Name => Base.Locator(".name");

    private ILocator Amount => Base.Locator(".amount");

    public async Task<Expense> GetExpense()
    {
        var name = await Name.InnerTextAsync();
        var amount = await Amount.InnerTextAsync();

        return new Expense(
            name, 
            decimal.Parse(amount, new CultureInfo("en-US")));
    }
}
