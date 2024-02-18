using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace AcceptanceTests.Drivers;

public class WebDriver : IExpenses
{
    private IPlaywright playwright = null!;
    private IBrowser browser = null!;
    private IPage page = null!;
    private readonly Uri websiteUrl;
    private readonly bool headlessMode;

    public WebDriver(IOptions<WebDriverOptions> options)
    {
        websiteUrl = options.Value.Uri;
        headlessMode = options.Value.Headless;
    }
    
    public async Task AddExpense(string name, decimal amount)
    {
        await page.Locator("#create").ClickAsync();
        
        await page.Locator("#Data1_Expense").FillAsync(name);
        
        await page.Locator("#Data1_Amount").FillAsync(amount.ToString(CultureInfo.InvariantCulture));
        
        await page.Locator("button[type='submit']").ClickAsync();
    }
    
    public async Task AssertExpenseIsVisibleAsync(string name, decimal amount)
    {
        await page.Locator("#home").ClickAsync();
        
        var expenses2 = (await page.Locator("#expenses .expense").AllAsync())
            .Select(async x => new Expense(
                await x.Locator(".name").InnerTextAsync(),
                decimal.Parse(await x.Locator(".amount").InnerTextAsync())))
            .ToArray();
        var result = await Task.WhenAll(expenses2);
        
        result
            .Should()
            .ContainEquivalentOf(new Expense(name, amount));
    }
    
    public async Task InitializeAsync()
    {
        playwright = await Playwright.CreateAsync();

        browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headlessMode,
            SlowMo = 500,
        });

        page = await browser.NewPageAsync();
        
        await page.GotoAsync(websiteUrl.ToString());
    }
    
    public async ValueTask DisposeAsync()
    {
        playwright.Dispose();
        await browser.DisposeAsync();
    }
}