using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public sealed class LoginPage(IPage page) 
    : PageBase(page)
{
    private ILocator Email => Base.Locator("#Input_Username");

    private ILocator Password => Base.Locator("#Input_Password");

    private ILocator LogInButton => Base.Locator("#login-submit");

    public async Task LogIn(string email, string password)
    {
        await Email.FillAsync(email);
        await Password.FillAsync(password);
        await LogInButton.ClickAsync();
    }
}
