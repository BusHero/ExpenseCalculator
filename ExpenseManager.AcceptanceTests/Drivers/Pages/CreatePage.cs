using System.Globalization;
using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public class CreatePage: PageBase
{
    public CreatePage(IPage page) : base(page) {}

    private ILocator Expense => Base.Locator("#Data1_Expense");

    private ILocator Amount => Base.Locator("#Data1_Amount");

    private ILocator Submit => Base.Locator("#submit-expense");
    
    public async Task CreateExpense(string name, decimal amount)
    {
        await Expense.FillAsync(name);
        
        await Amount.FillAsync(amount.ToString(CultureInfo.InvariantCulture));
        
        await Submit.ClickAsync();
    }
}
