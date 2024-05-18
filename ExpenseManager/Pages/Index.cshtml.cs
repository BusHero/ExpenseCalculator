using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    internal IndexDto? Data { get; private set; }

    [ModelBinder] public Data Data1 { get; set; } = null!;

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
            new Expense
            {
                Name = ExpenseName.FromString(Data1.Expense),
                Amount = Money.FromDecimal(Data1.Amount),
                Date = Data1.Date,
            });

        return Redirect("/");
    }

    public void OnGet(
        [FromServices] IApplicationService applicationService)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return;
        }

        var userId = User.FindFirstValue("sub");

        var expenses = applicationService
            .GetExpensesLoggedInUser(ExternalUserId.FromString(userId))
            .Select(x => new ExpenseDto
            {
                Expense = x.Name.Value,
                Amount = x.Amount.Value,
                Date = x.Date,
            })
            .OrderByDescending(x => x.Date)
            .ToList();

        Data = new IndexDto
        {
            Expenses = expenses,
        };
    }
}

internal sealed record IndexDto
{
    public required List<ExpenseDto> Expenses { get; init; }
}

internal sealed record ExpenseDto
{
    public required string Expense { get; init; }

    [DisplayFormat(DataFormatString = "{0:C}")]
    public required decimal Amount { get; init; }

    [DisplayFormat(DataFormatString = "{0:g}")]
    public required DateTime Date { get; init; }
}

public sealed record Data
{
    [Required] public string Expense { get; init; } = null!;

    [Required] public decimal Amount { get; init; }
    
    [Required] public DateTime Date { get; init; } = DateTime.Today;
}