using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class IndexModel : PageModel
{
    public IndexDto? Data { get; private set; }

    public void OnGet()
    {
        Data = new IndexDto([new ExpenseDto("Grocery", 123.12m)]);
    }
}

public record IndexDto(List<ExpenseDto> Expenses);

public record ExpenseDto(
    string Expense,
    decimal Amount);