using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

[IgnoreAntiforgeryToken(Order = 1001)]
public class Create : PageModel
{
    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        return Redirect("/");
    }
}
