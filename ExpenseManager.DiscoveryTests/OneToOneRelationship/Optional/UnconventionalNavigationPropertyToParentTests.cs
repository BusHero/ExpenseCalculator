using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship.Optional;

public sealed class UnconventionalNavigationPropertyToParentTests
{
    [Fact]
    public void NoConfigurationIsNotThrowing()
    {
        var context = ContextFactory.CreateContext<Context>(_ => {});

        AssertCanAddChildToParent(context);
    }
    
    [Fact]
    public void ConfigurePropertyFromParentShouldWork()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Parent>()
                .HasOne(x => x.Child)
                .WithOne(x => x.UnconventionalProperty)
                .HasForeignKey<Child>(x => x.ParentId)
                .IsRequired(false);
        });
        
        AssertCanAddChildToParent(context);
    }
    
    [Fact]
    public void ConfigurePropertyFromChild()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>()
                .HasOne(x => x.UnconventionalProperty)
                .WithOne(x => x.Child)
                .HasForeignKey<Child>(x => x.ParentId)
                .IsRequired(false);
        });
    
        AssertCanAddChildToParent(context);
    }

    private void AssertCanAddChildToParent(Context context)
    {
        var child = new Child();
        var parent = new Parent
        {
            Child = child,
        };

        context.Parent1.Add(parent);
    
        context.SaveChanges();
    
        child.UnconventionalProperty.Should().NotBeNull();
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
        
        public int ParentId { get; set; }
        
        public Parent? UnconventionalProperty { get; set; } 
    }
}
