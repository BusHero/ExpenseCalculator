using ExpenseManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.DataAccess;

public class EfExpensesStorage :
    IExpenseStorage
{
    public void Add(Expense expense)
    {
        throw new NotImplementedException();
    }
    
    public IEnumerable<Expense> GetAll() => throw new NotImplementedException();
    
    public void Save(User user)
    {
        throw new NotImplementedException();
    }
    
    public User? GetUser(UserId userId) => throw new NotImplementedException();
}
