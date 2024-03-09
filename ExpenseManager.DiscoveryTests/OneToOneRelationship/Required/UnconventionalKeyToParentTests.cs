using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests;

public sealed class UnconventionalKeyToParentTests
{
    [Fact]
    public void NoConfigurationIsThrowing()
    {
        var action = () => ContextFactory.CreateContext<Context>(_ => {});
        
        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void ConfigurePropertyFromParentShouldWork()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Parent>()
                .HasOne(x => x.Child)
                .WithOne(x => x.Parent)
                .HasForeignKey<Child>(x => x.TotallyUnconventionalKey)
                .IsRequired();
        });
        
        AssertCanAddChildToParent(context);
    }
    
    [Fact]
    public void ConfigurePropertyFromChild()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>()
                .HasOne(x => x.Parent)
                .WithOne(x => x.Child)
                .HasForeignKey<Child>(x => x.TotallyUnconventionalKey)
                .IsRequired();
        });

        AssertCanAddChildToParent(context);
    }

    private void AssertCanAddChildToParent(Context context)
    {
        var parent = new Parent();
        var child = new Child();
    
        parent.Child = child;
    
        context.Parent.Add(parent);
    
        context.SaveChanges();
    
        child.Parent.Should().NotBeNull();
    }
    
    public class Context: RelayContext<Context>, ITestContext<Context>
    {
        // ReSharper disable once MemberHidesStaticFromOuterClass
        public DbSet<Parent> Parent { get; init; } = null!;
        
        // ReSharper disable once MemberHidesStaticFromOuterClass
        public DbSet<Child> Child { get; init; } = null!;

        public Context(DbContextOptions<Context> options) : base(options)
        { }

        public static Context Create(
            DbContextOptions<Context> options)
            => new Context(options);
    }
    

    public class Parent
    {
        public int ParentId { get; set; }
        
        public Child? Child { get; set; }
    }
    
    public class Child
    {
        public int ChildId { get; set; }
        
        public int TotallyUnconventionalKey { get; set; }
        
        public Parent Parent { get; set; } = null!;
    }
}