using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship.OptionalShadowForeignKey;

public sealed class ConventionalKeysAndPropertiesTests: IDisposable
{
    private readonly Context context = ContextFactory.CreateContext<Context>(builder =>
    {
        builder.Entity<Parent>()
            .HasOne(x => x.Child)
            .WithOne(x => x.Parent)
            .HasForeignKey<Child>("ParentId");
    });
    
    public void Dispose()
    {
        context.Dispose();
    }
    
    [Fact]
    public void ChildPropertyIsSet()
    {
        using var context1 = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Parent>()
                .HasOne(x => x.Child)
                .WithOne(x => x.Parent)
                .HasForeignKey<Child>("ParentId");
        });
        
        var parent = new Parent();
        var child = new Child();

        parent.Child = child;

        context1.Parent1.Add(parent);

        context1.SaveChanges();

        child.Parent.Should().NotBeNull();
    }

    [Fact]
    public void ShadowPropertyIsSet()
    {
        using var context1 = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Parent>()
                .HasOne(x => x.Child)
                .WithOne(x => x.Parent)
                .HasForeignKey<Child>("ParentId");
        });
        
        var parent = new Parent();
        var child = new Child();

        parent.Child = child;

        context1.Parent1.Add(parent);

        context1.SaveChanges();
        
        context1
            .Entry(child)
            .Property("ParentId")
            .CurrentValue
            .Should()
            .NotBeNull();
    }
    
    [Fact]
    public void ParentToChildRelationshipIsOptional()
    {
        var parent = new Parent();

        context.Parent1.Add(parent);

        context.Invoking(x => x.SaveChanges())
            .Should()
            .NotThrow();
    }
    
    [Fact]
    public void ChildToParentRelationshipIsOptional()
    {
        var child = new Child();

        context.Child1.Add(child);

        context.Invoking(x => x.SaveChanges())
            .Should()
            .NotThrow();
    }
    
    private class Context: RelayContext<Context>, ITestContext<Context>
    {
        public DbSet<Parent> Parent1 { get; set; } = null!;
        
        public DbSet<Child> Child1 { get; set; } = null!;

        public Context(DbContextOptions<Context> options) : base(options)
        { }
        
        public static Context Create(DbContextOptions<Context> options) 
            => new Context(options);
    }

    private class Parent
    {
        public int Id { get; set; }
        
        public Child? Child { get; set; }
    }
    
    private class Child
    {
        public int Id { get; set; }
        
        public Parent? Parent { get; set; }
    }
}