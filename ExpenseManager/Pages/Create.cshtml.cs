using System.ComponentModel.DataAnnotations;
using ExpenseManager.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class Create : PageModel
{
    [ModelBinder]
    public Data Data1 { get; set; } = null!;
    
    public void OnGet()
    {
    }

    public IActionResult OnPost(
        [FromServices] IExpenseStorage storage)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        storage.Add(new Expense
        {
            Name= ExpenseName.FromString(Data1.Expense),
            Amount    = Money.FromDecimal(Data1.Amount),
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
