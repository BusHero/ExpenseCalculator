using AcceptanceTests.Drivers.Pages;
using Microsoft.Playwright;

namespace AcceptanceTests.Drivers;

public sealed class EnsureUserLogInAction(IPage page)
{
    public async Task LogInUser(User user)
    {
        var navBar = new NavBar(page);
        var loggedInUser = await navBar.GetLoggedInUser();

        if (loggedInUser == user.Email)
        {
            return;
        }
        if (loggedInUser is not null)
        {
            await navBar.LogoutUser();
        }

        await navBar.NavigateLoginPage();
        
        loggedInUser = await navBar.GetLoggedInUser();
        
        if (loggedInUser == user.Email)
        {
            return;
        }
        if (loggedInUser is not null)
        {
            await navBar.LogoutUser();
        }

        await navBar.NavigateLoginPage();

        var loginPage = new LoginPage(page);

        await loginPage.LogIn(user.Email, user.Password);
    }
}
