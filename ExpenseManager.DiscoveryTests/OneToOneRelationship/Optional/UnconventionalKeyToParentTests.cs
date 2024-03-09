using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship.Optional;

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
    
        context.Parent1.Add(parent);
    
        context.SaveChanges();
    
        child.Parent.Should().NotBeNull();
    }
    
    private class Context: RelayContext<Context>, ITestContext<Context>
    {
        public DbSet<Parent> Parent1 { get; init; } = null!;
        
        public DbSet<Child> Child1 { get; init; } = null!;

        public Context(DbContextOptions<Context> options) : base(options)
        { }

        public static Context Create(
            DbContextOptions<Context> options)
            => new Context(options);
    }
    

    private class Parent
    {
        public int ParentId { get; set; }
        
        public Child? Child { get; set; }
    }
    
    private class Child
    {
        public int ChildId { get; set; }
        
        public int TotallyUnconventionalKey { get; set; }
        
        public Parent? Parent { get; set; }
    }
}