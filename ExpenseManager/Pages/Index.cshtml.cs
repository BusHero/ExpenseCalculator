using ExpenseManager.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class IndexModel : PageModel
{
    public IndexDto? Data { get; private set; }

    public void OnGet(
        [FromServices] IExpenseStorage expenseStorage)
    {
        Data = new IndexDto(
            expenseStorage
                .GetAll()
                .Select(x => new ExpenseDto(x.Name.Value, x.Amount.Value))
                .ToList());
    }
}

public record IndexDto(List<ExpenseDto> Expenses);

public record ExpenseDto(
    string Expense,
    decimal Amount);