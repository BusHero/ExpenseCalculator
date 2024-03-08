using ExpenseManager.Domain;
using Microsoft.AspNetCore.Identity;

namespace ExpensesManager.DataAccess;

public class ApplicationUser: IdentityUser
{
    public User User { get; set; }
}