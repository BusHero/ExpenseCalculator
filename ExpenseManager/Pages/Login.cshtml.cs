using ExpenseManager.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class Login : PageModel
{
    public async Task<IActionResult> OnGet(
        [FromServices] IApplicationService applicationService)
    {
        var result = await HttpContext.AuthenticateAsync();

        if (!result.Succeeded)
        {
            throw new Exception("User not logged in");
        }

        var id = result.Principal.FindFirst("sub")?.Value!;

        await applicationService.CreateNewUser(
            ExternalUserId.FromString(id));
        
        return Redirect("/");
    }
}
