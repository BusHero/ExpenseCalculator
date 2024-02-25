using AcceptanceTests.Drivers.Pages;
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
        var navBar = new NavBar(page);
        var createPage = new CreatePage(page);
        
        await navBar.NavigateCreatePage();

        await createPage.CreateExpense(name, amount);
    }
    
    public async Task AssertExpenseIsVisibleAsync(string name, decimal amount)
    {
        var navBar = new NavBar(page);
        
        var homePage = new HomePage(page);

        await navBar.NavigateHome();
        
        var result = await homePage.GetExpenses();
        
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
    
    public async Task RegisterUserAsync(string email, string password)
    {
        var navBar = new NavBar(page);
        var registerPage = new RegisterPage(page);
        var registerConfirm = new RegisterConfirmPage(page);
        
        await navBar.NavigateRegisterPage();

        await registerPage.RegisterUser(
            new User(
                email,
                password,
                "foo"));

        await registerConfirm.ConfirmAccount();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        playwright.Dispose();
        await browser.DisposeAsync();
    }
}