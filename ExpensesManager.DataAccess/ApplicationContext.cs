using ExpenseManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.DataAccess;

public sealed class ApplicationContext(DbContextOptions<ApplicationContext> options) 
    : DbContext(options)
{
    public DbSet<User> DomainUsers { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id) ;

            b.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasConversion(x => x.Id, x => UserId.FromInt(x));

            b.Property(x => x.ExternalId)
                .HasConversion(x => x.Value, x => ExternalUserId.FromString(x))
                .IsRequired();

            b.HasIndex(x => x.ExternalId)
                .IsUnique();

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
