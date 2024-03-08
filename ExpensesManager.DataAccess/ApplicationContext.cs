using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.DataAccess;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : base(options)
    {
    }

    public DbSet<Expense2> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

public class Expense2
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;
    
    public decimal Amount { get; set; }
}


