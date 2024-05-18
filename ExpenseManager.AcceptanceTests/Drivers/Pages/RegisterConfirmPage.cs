using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public sealed class RegisterConfirmPage(IPage page) : PageBase(page)
{
    private ILocator ConfirmLink => Base.Locator("#confirm-link");
    
    public async Task ConfirmAccount() => await ConfirmLink.ClickAsync();
}
