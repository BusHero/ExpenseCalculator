using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public class NavBar : PageBase
{
    public NavBar(IPage page): base(page) { }

    private ILocator Home => Base.Locator("#home");

    private ILocator Create => Base.Locator("#create");

    private ILocator Register => Base.Locator("#register");

    public async Task NavigateHome()
    {
        await Home.ClickAsync();
    }

    public async Task NavigateCreatePage()
    {
        await Create.ClickAsync();
    }

    public async Task NavigateRegisterPage()
    {
        await Register.ClickAsync();
    }
}
