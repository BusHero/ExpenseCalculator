using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public class RegisterConfirmPage: PageBase
{
    public RegisterConfirmPage(IPage page): base(page) { }

    private ILocator ConfirmLink => Base.Locator("#confirm-link");
    
    public async Task ConfirmAccount()
    {
        await ConfirmLink.ClickAsync();
    }
}
