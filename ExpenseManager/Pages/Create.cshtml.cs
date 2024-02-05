using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

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
