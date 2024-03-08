using ExpenseManager.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.DataAccess;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : base(options)
    {
    }

    public DbSet<User> Users2 { get; init; } = null!;
    public DbSet<User2> Users3 { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id) ;

            b.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasConversion(x => x.Id, x => UserId.FromInt(x));

            b.OwnsMany(x => x.Expenses,
                b1 =>
                {
                    b1.Property(x => x.Name)
                        .HasConversion(x => x.Value, x => ExpenseName.FromString(x));

                    b1.Property(x => x.Amount)
                        .HasConversion(x => x.Value, x => Money.FromDecimal(x));
                    
                    b1.ToJson();
                });
        });
    }
}