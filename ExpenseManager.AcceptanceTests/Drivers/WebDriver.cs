using AcceptanceTests.Drivers.Pages;
using AutoFixture;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace AcceptanceTests.Drivers;

public class WebDriver : IExpenses, IAsyncDisposable
{
    private IPlaywright playwright = null!;
    private IBrowser browser = null!;
    private IPage page = null!;
    private readonly Uri websiteUrl;
    private readonly bool headlessMode;
    private readonly Dictionary<string, User> users = new ();
    private readonly Dictionary<string, Expense> expenses = new ();
    private readonly Fixture fixture;

    public WebDriver(IOptions<WebDriverOptions> options)
    {
        websiteUrl = options.Value.Uri;
        headlessMode = options.Value.Headless;

        fixture = new Fixture();
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
    
    public async Task AddExpense(string userId, string expenseId)
    {
        var user = users[userId];
        
        var name = fixture.Create<string>();
        var amount = fixture.Create<decimal>();
        var expense = new Expense(
            name, 
            amount);
        expenses[expenseId] = expense;

        await PerformAddExpense(user, expense);
    }
    
    private async Task PerformAddExpense(
        User user,
        Expense expense)
    {
        var navBar = new NavBar(page);
        var createPage = new CreatePage(page);
        var action = new EnsureUserLogInAction(page);

        await action.LogInUser(user);
        
        await navBar.NavigateCreatePage();

        await createPage.CreateExpense(expense.Name, expense.Amount);
    }
    
    public async Task AssertExpenseIsVisibleAsync(string userId, string expenseId)
    {
        var user = users[userId];
        
        var expense = expenses[expenseId];
        
        var result = await GetExpenses(user);
        
        result
            .Should()
            .ContainEquivalentOf(expense);
    }
    
    public async Task AssertExpenseIsNotVisibleAsync(string userId, string expenseId)
    {
        var user = users[userId];
        
        var expense = expenses[expenseId];
        
        var result = await GetExpenses(user);

        result.Should().NotContainEquivalentOf(expense);
    }

    private async Task<Expense[]> GetExpenses(User user)
    {
        var navBar = new NavBar(page);
        
        var homePage = new HomePage(page);
        var action = new EnsureUserLogInAction(page);

        await action.LogInUser(user);
        
        await navBar.NavigateHome();
        
        var result = await homePage.GetExpenses();

        return result;
    }

    public async Task RegisterUserAsync(string userId)
    {
        var email = $"{fixture.Create<string>()}@example.com";
        var name = fixture.Create<string>();
        const string password = "P@ssword1";
        
        this.users[userId] = new User(email,password, name);
        
        await PerformRegistrationAsync(email, password, name);
    }

    private async Task PerformRegistrationAsync(string email, string password, string name)
    {
        var navBar = new NavBar(page);
        var registerPage = new RegisterPage(page);
        var registerConfirm = new RegisterConfirmPage(page);
        
        await navBar.NavigateRegisterPage();

        await registerPage.RegisterUser(
            new User(
                email,
                password,
                name));

        // await registerConfirm.ConfirmAccount();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await browser.DisposeAsync();
        playwright.Dispose();
    }
}