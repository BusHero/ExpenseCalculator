using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DataAccess.Tests.DiscoveryTests;

public class CachingTests
{
    [Fact]
    public void ModelIsCachedByDefault()
    {
        var invocations1 = 0;
        var invocations2 = 0;
        
        using var c1= ContextFactory.CreateContext<Context>(x => { invocations1++; });
        using var c2 = ContextFactory.CreateContext<Context>(x => { invocations2++; });
        
        using var _ = new AssertionScope();
        invocations1.Should().Be(2);
        invocations2.Should().Be(0);
    }
    
    public class Context: RelayContext<Context>, ITestContext<Context>
    {
        public DbSet<Entity> Entity1 { get; set; } = null!;

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public static Context Create(
            DbContextOptions<Context> options)
            => new Context(options)
            {
                CacheModel = true,
            };
    }

    public class Entity { public int Id { get; set; } }
}
