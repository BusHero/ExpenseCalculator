using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests;

public sealed class RequiredOneToOneTests : IDisposable
{
    private readonly Context context 
        = ContextFactory.CreateContext<Context>();

    public void Dispose() => context.Dispose();

    [Fact]
    public void ChildPropertyIsSet()
    {
        var parent = new Parent();
        var child = new Child();

        parent.Child = child;

        context.Parent.Add(parent);

        context.SaveChanges();

        child.Parent.Should().NotBeNull();
    }
    
    [Fact]
    public void ParentToChildRelationshipIsOptional()
    {
        var parent = new Parent();

        context.Parent.Add(parent);

        context.Invoking(x => x.SaveChanges())
            .Should()
            .NotThrow<Exception>();
    }
    
    [Fact]
    public void ChildToParentRelationshipIsMandatory()
    {
        var child = new Child();

        context.Child.Add(child);

        context.Invoking(x => x.SaveChanges())
            .Should()
            .Throw<Exception>();
    }
    
    public class Context: DbContext, ITestContext<Context>
    {
        public DbSet<Parent> Parent { get; set; } = null!;
        
        public DbSet<Child> Child { get; set; } = null!;

        public Context(DbContextOptions<Context> options) : base(options)
        { }
        
        public static Context Create(DbContextOptions<Context> options) 
            => new Context(options);
    }

    public class Parent
    {
        public int Id { get; set; }
        
        public Child? Child { get; set; }
    }
    
    public class Child
    {
        public int Id { get; set; }
        
        public int ParentId { get; set; }
        
        public Parent Parent { get; set; } = null!;
    }
}