using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public sealed class NavBar(IPage page) : PageBase(page)
{
    private ILocator Home => Base.Locator("#home");

    private ILocator Create => Base.Locator("#create");

    private ILocator Register => Base.Locator("#register");
    
    private ILocator Login => Base.Locator("#login");

    private ILocator Username => Base.Locator("#logged-in-user");
    
    private ILocator Logout => Base.Locator("#logout");

    public async Task NavigateHome() 
        => await Home.ClickAsync();

    public async Task NavigateCreatePage() 
        => await Create.ClickAsync();

    public async Task NavigateRegisterPage() 
        => await Register.ClickAsync();

    public async Task NavigateLoginPage() 
        => await Login.ClickAsync();

    public async Task<string?> GetLoggedInUser()
    {
        return await Username.CountAsync() switch
        {
            1 => await Username.InnerTextAsync(),
            _ => null,
        };
    }

    public async Task LogoutUser() 
        => await Logout.ClickAsync();
}
