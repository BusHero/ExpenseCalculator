using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class Login : PageModel
{
    public IActionResult OnGet()
    {
        SignIn(User, "Cookies");
        return Redirect("/");
    }
}
