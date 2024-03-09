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
    
        context.Parent.Add(parent);
    
        context.SaveChanges();
    
        child.Parent.Should().NotBeNull();
    }
    
    private void AssertCanAddParentToChild(Context context)
    {
        var parent = new Parent();
        var child = new Child();

        child.Parent = parent;
    
        context.Child.Add(child);
    
        context.SaveChanges();

        parent.UnconventionalProperty.Should().NotBeNull();
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
        
        public Child? UnconventionalProperty { get; set; }
    }
    
    public class Child
    {
        public int ChildId { get; set; }
        
        public int ParentId { get; set; }
        
        public Parent Parent { get; set; } = null!;
    }
}