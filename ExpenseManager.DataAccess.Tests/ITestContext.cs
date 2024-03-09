using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.DataAccess.Tests;

public interface ITestContext<T> where T: DbContext
{
    abstract static T Create(DbContextOptions<T> options);
}

public interface ITestContext2<T> where T: DbContext
{
    abstract static T CreateContext(
        DbContextOptions<T> options,
        Action<ModelBuilder> delegate1);
}
