using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests.OneToOneRelationship;

public class RequiredWithValueObjectChildKey
{
    [Fact]
    public void Foo()
    {
        var context = ContextFactory.CreateContext<Context>(builder =>
        {
            builder.Entity<Child>(x =>
            {
                x.HasKey(x => x.Id);

                x.Property(y => y.Id)
                    .HasConversion(x1 => x1.Id, x1 => new ChildId(x1));

                x.HasOne<Parent>()
                    .WithOne(y => y.Child)
                    .HasForeignKey<Child>("ApplicationUser");
            });
        });
        
        AssignChildToParent(
            context, 
            (child, parent) => context.Entry(child).Property("ApplicationUser").CurrentValue.Should().Be(parent.Id));
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
        public ChildId Id { get; set; }
    }

    private record struct ChildId(int Id);
}
