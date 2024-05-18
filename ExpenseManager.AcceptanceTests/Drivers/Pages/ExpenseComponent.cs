using System.Globalization;
using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public sealed class ExpenseComponent(ILocator baseLocator) 
    : PageBase(baseLocator)
{
    private ILocator Name => Base.Locator(".name");

    private ILocator Amount => Base.Locator(".amount");

    public async Task<Expense> GetExpense()
    {
        var name = await Name.InnerTextAsync();
        var amount = await Amount.InnerTextAsync();

        return new Expense(
            name, 
            decimal.Parse(amount[1..], new CultureInfo("en-US")));
    }
}
