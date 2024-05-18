using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExpenseManager.DiscoveryTests;

public abstract class RelayContext<T>(DbContextOptions<T> options) 
    : DbContext(options)
    where T : DbContext
{
    protected bool CacheModel { get; set; }

    public Action<ModelBuilder> ModelCreatingDelegate { get; set; } = _ => {};

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

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class NoCacheModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime) => new();
    }
}
