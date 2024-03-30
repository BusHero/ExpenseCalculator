using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class Signout : PageModel
{
    public IActionResult OnGet() 
        => SignOut("oidc", "Cookies");
}
