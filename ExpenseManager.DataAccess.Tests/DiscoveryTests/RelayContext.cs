using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExpenseManager.DataAccess.Tests.DiscoveryTests;

public class RelayContext<T>: DbContext
    where T: DbContext
{
    public RelayContext(DbContextOptions<T> options) : base(options)
    { }

    protected bool CacheModel { get; set; } = false;

    public Action<ModelBuilder> ModelCreatingDelegate { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelCreatingDelegate(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        if (!CacheModel)
        {
            optionsBuilder.ReplaceService<
                IModelCacheKeyFactory, 
                NoCacheModelCacheKeyFactory>();
        }
    }
}
