using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class Create : PageModel
{
    [ModelBinder]
    public Data Data1 { get; set; } = null!;

    public void OnGet()
    {}

    public IActionResult OnPost(
        [FromServices] ApplicationService applicationService,
        [FromServices] ApplicationContext context,
        [FromServices] IExpenseStorage storage)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        applicationService.AddExpenseToLoggedInUser(
            LoggedInUserId.FromString(userId),
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
