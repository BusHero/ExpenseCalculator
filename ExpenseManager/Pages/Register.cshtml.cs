using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

[AllowAnonymous]
public class Register : PageModel
{
    public IActionResult OnGet()
    {
        return Redirect("https://localhost:5001/Identity/Account/Register");
    }
}
