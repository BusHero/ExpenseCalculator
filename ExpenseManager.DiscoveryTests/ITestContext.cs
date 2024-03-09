using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests;

public interface ITestContext<T> where T: DbContext
{
    abstract static T Create(DbContextOptions<T> options);
}
