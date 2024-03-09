using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExpenseManager.DiscoveryTests;

public abstract class RelayContext<T>: DbContext
    where T: DbContext
{
    protected RelayContext(DbContextOptions<T> options) : base(options)
    { }

    protected bool CacheModel { get; set; }

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

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class NoCacheModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime) 
            => new object();
    }
}
