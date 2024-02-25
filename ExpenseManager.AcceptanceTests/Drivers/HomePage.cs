using Microsoft.Playwright;

namespace AcceptanceTests.Drivers;

public class HomePage : PageBase
{
    public HomePage(IPage page) : base(page) {}

    private ILocator Expense => Base.Locator("#expenses .expense");
    
    public async Task<Expense[]> GetExpenses()
    {
        var expenses2 = (await Expense.AllAsync())
            .Select(x => new ExpenseComponent(x).GetExpense())
            .ToArray();
        
        var result = await Task.WhenAll(expenses2);

        return result;
    }
}
