using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship.RequiredWithoutNavigationToPrimary;

public sealed class ConventionalKeysAndPropertiesTests
{
    [Fact]
    public void NoConfiguration_DoesntLinkPrimaryKeys()
    {
        var context = ContextFactory.CreateContext<Context>(_ => {});
        
        AssignChildToParent(
            context, 
            (child, parent) => child.Id.Should().NotBe(parent.Id));
    }
    
    [Fact]
    public void ChildConfigurationSetUpPrimaryKeysCorrectly()
    {
        var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>()
                .HasOne<Parent>()
                .WithOne(x => x.Child)
                .HasForeignKey<Child>();
        });
        
        AssignChildToParent(
            context, 
            (child, parent) => child.Id.Should().Be(parent.Id));
    }
    
    [Fact]
    public void ParentConfigurationSetUpPrimaryKeysCorrectly()
    {
        var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Parent>()
                .HasOne(x => x.Child)
                .WithOne()
                .HasForeignKey<Child>();
        });
        
        AssignChildToParent(
            context, 
            (child, parent) => child.Id.Should().Be(parent.Id));
    }
    
    private void AssignChildToParent(
        Context context,
        Action<Child, Parent> action)
    {
        var fixture = new Fixture();
        var id = fixture.Create<int>();

        var parent = new Parent { Id = id };
        var child = new Child();

        parent.Child = child;

        context.Parent1.Add(parent);

        context.SaveChanges();

        action(child, parent);
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
    }
}