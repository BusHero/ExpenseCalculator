using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.DataAccess;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : base(options)
    {
    }

    public DbSet<Expense2> Expenses { get; init; } = null!;
}