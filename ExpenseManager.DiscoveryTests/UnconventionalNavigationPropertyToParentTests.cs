using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests;

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
                .HasOne(x => x.UnconventionalProperty)
                .WithOne(x => x.Child)
                .HasForeignKey<Child>(x => x.ParentId)
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
    
        child.UnconventionalProperty.Should().NotBeNull();
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
        
        public int ParentId { get; set; }
        
        public Parent UnconventionalProperty { get; set; } = null!;
    }
}
