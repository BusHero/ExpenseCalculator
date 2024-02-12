namespace AcceptanceTests.Drivers;

public class WebDriver : IDriver
{
    public WebDriver()
    {
        
    }
    
    public Task AddExpense(Expense expense) => throw new NotImplementedException();
    
    public Task<List<Expense>?> GetExpenses() => throw new NotImplementedException();
    
    public void NavigateExpensesPage()
    {
        // await Page.GotoAsync("https://expensemanager2.azurewebsites.net/");
        //
        // var getStarted = Page.Locator("#create");
        //
        // await getStarted.ClickAsync();
        //
        // var expense = Page.Locator("#Data1_Expense");
        // var amount = Page.Locator("#Data1_Amount");
        //
        // await expense.FillAsync("Foo");
        // await amount.FillAsync(123.123.ToString(CultureInfo.InvariantCulture));
        //
        // var submit = Page.Locator("button[type='submit']");
        //
        // await submit.ClickAsync();
    }
    public void Initialize()
    {
    }
}
