using Microsoft.AspNetCore.Identity;

namespace ExpensesManager.DataAccess;

public class ApplicationUser: IdentityUser
{
    public required List<Expense2> Expenses { get; set; } = [];
}