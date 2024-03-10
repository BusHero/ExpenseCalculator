using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship.WithoutNavigationToPrincipal;

public class UnconventionalForeignKeyTests
{
    private readonly IFixture fixture = new Fixture();
    
    [Fact]
    public void NoConfigurationDoesNotSetNavigationalProperty()
    {
        var context = ContextFactory.CreateContext<Context>();
        AssignChildToParent(
            context,
            (child, parent) => child.UnconventionalProperty.Should().NotBe(parent.Id));
    }

    [Fact]
    public void ParentConfigurationSetsNavigationalProperty()
    {
        var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Parent>()
                .HasOne(x => x.Child)
                .WithOne()
                .HasForeignKey<Child>(x => x.UnconventionalProperty)
                .IsRequired();
        });
        
        AssignChildToParent(
            context,
            (child, parent) => child.UnconventionalProperty.Should().Be(parent.Id));
    }
    
    [Fact]
    public void ChildConfigurationSetsForeignKey()
    {
        var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>()
                .HasOne<Parent>()
                .WithOne(x => x.Child)
                .HasForeignKey<Child>(x => x.UnconventionalProperty)
                .IsRequired();
        });
        
        AssignChildToParent(
            context,
            (child, parent) => child.UnconventionalProperty.Should().Be(parent.Id));
    }
    
    private void AssignChildToParent(
        Context context,
        Action<Child, Parent> action)
    {
        var child = new Child();
        var parent = new Parent
        {
            Id = fixture.Create<int>(),
            Child = child,
        };

        context.Parent1.Add(parent);

        context.SaveChanges();

        action(child, parent);
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
        
        public int UnconventionalProperty { get; set; }
    }
}
