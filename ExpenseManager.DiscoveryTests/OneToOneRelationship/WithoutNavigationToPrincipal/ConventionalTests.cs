using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship.WithoutNavigationToPrincipal;

public class ConventionalTests
{
    private readonly IFixture fixture = new Fixture();
    
    [Fact]
    public void NoConfigurationWorks()
    {
        using var context1 = ContextFactory.CreateContext<Context>();

        var parent = new Parent { Id = fixture.Create<int>(), };
        var child = new Child();
        parent.Child = child;

        context1.Parent1.Add(parent);

        context1.SaveChanges();

        child.ParentId.Should().Be(parent.Id);
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
        
        public int ParentId { get; set; }
    }
}
