using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests;

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