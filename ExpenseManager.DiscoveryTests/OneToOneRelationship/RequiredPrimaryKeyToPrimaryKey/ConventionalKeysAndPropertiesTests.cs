using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship.RequiredPrimaryKeyToPrimaryKey;

public sealed class ConventionalKeysAndPropertiesTests
{
    [Fact]
    public void NoConfiguration_Throws()
    {
       var action = () => ContextFactory.CreateContext<Context>(_ => {});

       action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void ParentToChildWorks()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Parent>()
                .HasOne(x => x.Child)
                .WithOne(x => x.Parent)
                .HasForeignKey<Child>();
        });

        AssertChildAndParentHaveSameKey(context);
    }
    
    [Fact]
    public void ChildToParentWorks()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>()
                .HasOne(x => x.Parent)
                .WithOne(x => x.Child)
                .HasForeignKey<Child>();
        });

        AssertChildAndParentHaveSameKey(context);
    }
    
    [Fact]
    public void ParentToChildRelationshipIsOptional()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>()
                .HasOne(x => x.Parent)
                .WithOne(x => x.Child)
                .HasForeignKey<Child>();
        });
        var parent = new Parent();

        context.Parent1.Add(parent);

        context.Invoking(x => x.SaveChanges())
            .Should()
            .NotThrow();
    }
    
    [Fact]
    public void ChildToParentRelationshipIsMandatory()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>()
                .HasOne(x => x.Parent)
                .WithOne(x => x.Child)
                .HasForeignKey<Child>();
        });
        var child = new Child();

        context.Child1.Add(child);

        context.Invoking(x => x.SaveChanges())
            .Should()
            .Throw<Exception>();
    }
    
    [Fact]
    public void ChildIsMandatory()
    {
        using var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>()
                .HasOne(x => x.Parent)
                .WithOne(x => x.Child)
                .HasForeignKey<Child>();
        });

        AssertChildAndParentHaveSameKey(context);
    }
    

    private void AssertChildAndParentHaveSameKey(Context context)
    {
        var fixture = new Fixture();
        var id = fixture.Create<int>();

        var parent = new Parent { Id = id };
        var child = new Child();

        parent.Child = child;

        context.Parent1.Add(parent);

        context.SaveChanges();

        child.Id.Should().Be(id);
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
        
        public Parent Parent { get; set; } = null!;
    }
}