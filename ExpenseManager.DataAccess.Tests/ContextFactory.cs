using ExpenseManager.DataAccess.Tests.DiscoveryTests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DataAccess.Tests;

public static class ContextFactory
{
    public static T CreateContext<T>() where T : DbContext, ITestContext<T>
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<T>()
            .UseSqlite(connection)
            .Options;

        var context = T.Create(options);

        context.Database.EnsureCreated();

        return context;
    }
    
    public static T CreateContext<T>(Action<ModelBuilder> onContextCreating) 
        where T : RelayContext<T>, ITestContext<T>
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<T>()
            .UseSqlite(connection)
            .Options;

        var context = T.Create(options);
        context.ModelCreatingDelegate = onContextCreating;

        context.Database.EnsureCreated();

        return context;
    }
}


public class RelayModelCreatingContext<T> : DbContext
    where T: DbContext, ITestContext2<T>
{
    public RelayModelCreatingContext( DbContextOptions<T> options )
        : base(options)
    { }

    public Action<ModelBuilder> ModelCreatingDelegate { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelCreatingDelegate(modelBuilder);
    }
}