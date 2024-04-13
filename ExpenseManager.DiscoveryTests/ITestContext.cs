using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DiscoveryTests;

public interface ITestContext<T> where T: DbContext
{
    static abstract T Create(DbContextOptions<T> options);
}
