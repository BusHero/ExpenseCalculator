using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager;

public interface IApplicationService
{
    void AddExpenseToLoggedInUser(
        ExternalUserId externalUserId,
        Expense expense);
    
    IEnumerable<Expense> GetExpensesLoggedInUser(ExternalUserId externalUserId);
    
    Task CreateNewUser(ExternalUserId id);
}

public class ApplicationService : IApplicationService
{
    private readonly ApplicationContext context;
    
    public ApplicationService(ApplicationContext context)
    {
        this.context = context;
    }

    public void AddExpenseToLoggedInUser(
        ExternalUserId externalUserId,
        Expense expense)
    {
        var user = context
            .DomainUsers
            .First(x => x.ExternalId == externalUserId);

        user.AddExpense(expense);
        
        context.SaveChanges();
    }
    public IEnumerable<Expense> GetExpensesLoggedInUser(ExternalUserId externalUserId)
    {
        var user = context
            .DomainUsers
            .Include(user => user.Expenses)
            .Single(x => x.ExternalId == externalUserId);

        return user.Expenses;
    }
    
    public async Task CreateNewUser(ExternalUserId id)
    {
        var user = await context.DomainUsers
            .SingleOrDefaultAsync(x => x.ExternalId == id);

        if (user is not null)
        {
            return;
        }
        
        var newUser = User.CreateNew(id);
        
        context.DomainUsers.Add(newUser);
        
        await context.SaveChangesAsync();
    }
}