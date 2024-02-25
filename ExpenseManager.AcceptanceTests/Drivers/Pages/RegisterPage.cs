using Microsoft.Playwright;

namespace AcceptanceTests.Drivers.Pages;

public class RegisterPage: PageBase
{
    public RegisterPage(IPage page): base(page) { }

    private ILocator Email => Base.Locator("#Input_Email");
    
    private ILocator Name => Base.Locator("#Input_Name");
    
    private ILocator Password => Base.Locator("#Input_Password");
    
    private ILocator ConfirmPassword=> Base.Locator("#Input_ConfirmPassword");
    
    private ILocator Register => Base.Locator("#registerSubmit");
    
    public async Task RegisterUser(User user)
    {
        await Email.FillAsync(user.Email);
        await Name.FillAsync(user.Name);
        await Password.FillAsync(user.Password);
        await ConfirmPassword.FillAsync(user.Password);
        await Register.ClickAsync();
    }
}
