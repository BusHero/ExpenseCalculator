using System.Security.Claims;
using ExpenseManager.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class IndexModel : PageModel
{
    public IndexDto? Data { get; private set; }

    public void OnGet(
        [FromServices] IApplicationService applicationService)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return;
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var expenses = applicationService
            .GetExpensesLoggedInUser(LoggedInUserId.FromString(userId))
            .Select(x => new ExpenseDto(x.Name.Value, x.Amount.Value))
            .ToList();

        Data = new(expenses);
    }
}

public record IndexDto(List<ExpenseDto> Expenses);

public record ExpenseDto(
    string Expense,
    decimal Amount);