#nullable disable

using System.Text;
using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace ExpenseManager.Areas.Identity.Pages.Account;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ApplicationContext context;

    public ConfirmEmailModel(
        UserManager<ApplicationUser> userManager,
        ApplicationContext context)
    {
        this.userManager = userManager;
        this.context = context;
    }

    [TempData]
    public string StatusMessage { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return RedirectToPage("/Index");
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            user.User = new User();
            await context.SaveChangesAsync();
        }
        
        StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
        return Page();
    }
}