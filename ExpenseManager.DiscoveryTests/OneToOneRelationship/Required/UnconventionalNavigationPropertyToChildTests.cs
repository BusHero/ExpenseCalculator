using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship.Required;

public sealed class UnconventionalNavigationPropertyToChildTests
{
    [Fact]
    public void NoConfiguration_CanAddChildToParent()
    {
        var context = ContextFactory.CreateContext<Context>(_ => {});

        AssertCanAddChildToParent(context);
    }
    
    [Fact]
    public void NoConfiguration_CanAddParentToChild()
    {
        var context = ContextFactory.CreateContext<Context>(_ => {});

        AssertCanAddParentToChild(context);
    }
    
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
                .HasOne(x => x.UnconventionalProperty)
                .WithOne(x => x.Parent)
                .HasForeignKey<Child>(x => x.ParentId)
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
                .WithOne(x => x.UnconventionalProperty)
                .HasForeignKey<Child>(x => x.ParentId)
                .IsRequired();
        });
    
        AssertCanAddChildToParent(context);
    }

    private void AssertCanAddChildToParent(Context context)
    {
        var parent = new Parent();
        var child = new Child();
    
        parent.UnconventionalProperty = child;
    
        context.Parent1.Add(parent);
    
        context.SaveChanges();
    
        child.Parent.Should().NotBeNull();
    }
    
    private void AssertCanAddParentToChild(Context context)
    {
        var parent = new Parent();
        var child = new Child();

        child.Parent = parent;
    
        context.Child1.Add(child);
    
        context.SaveChanges();

        parent.UnconventionalProperty.Should().NotBeNull();
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
        
        public Child? UnconventionalProperty { get; set; }
    }
    
    private class Child
    {
        public int ChildId { get; set; }
        
        public int ParentId { get; set; }

        public Parent Parent { get; set; } = null!;
    }
}
