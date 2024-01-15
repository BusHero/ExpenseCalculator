using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class IndexModel : PageModel {
    private readonly ILogger<IndexModel> _logger;

    public IndexDto Data { get; private set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        Data = null!;
        _logger = logger;
    }

    public void OnGet()
    {
        Data = new IndexDto([new ExpenseDto("Grocery", 123.12m)]);
    }
}

public record IndexDto(List<ExpenseDto> Expenses);

public record ExpenseDto(
    string Expense,
    decimal Amount);