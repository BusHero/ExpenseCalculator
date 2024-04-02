using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

[Authorize]
public class Create : PageModel
{
    [ModelBinder]
    public Data Data1 { get; set; } = null!;

    public void OnGet()
    {}

    public async Task<IActionResult> OnPost(
        [FromServices] IApplicationService applicationService,
        [FromServices] ApplicationContext context)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var result = await HttpContext.AuthenticateAsync();

        if (!(result.Succeeded && (result.Principal.Identity?.IsAuthenticated ?? false)))
        {
            return Forbid();
        }
        // if (!result.Succeeded || !(result.Principal.Identity?.IsAuthenticated ?? false))
        // {
        //     return Forbid();
        // }
        
        var userId = User.FindFirstValue("sub") ?? string.Empty;

        applicationService.AddExpenseToLoggedInUser(
            ExternalUserId.FromString(userId),
            new()
            {
                Name = ExpenseName.FromString(Data1.Expense),
                Amount = Money.FromDecimal(Data1.Amount),
            });
        
        return Redirect("/");
    }

    public class Data
    {
        [Required]
        public string Expense { get; init; } = null!;

        [Required]
        public decimal Amount { get; init; }
    }
}
