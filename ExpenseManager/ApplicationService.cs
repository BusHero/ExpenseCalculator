using ExpenseManager.Domain;
using ExpensesManager.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager;

public interface IApplicationService
{
    void AddExpenseToLoggedInUser(
        LoggedInUserId loggedInUserId,
        Expense expense);
    
    IEnumerable<Expense> GetExpensesLoggedInUser(LoggedInUserId loggedInUserId);
}

public class ApplicationService : IApplicationService
{
    private readonly ApplicationContext context;
    
    public ApplicationService(ApplicationContext context)
    {
        this.context = context;
    }

    public void AddExpenseToLoggedInUser(
        LoggedInUserId loggedInUserId,
        Expense expense)
    {
        var user = context
            .Users
            .Include(x => x.User)
            .Single(x => x.Id == loggedInUserId.Value);

        user.User ??= new();

        user.User.AddExpense(expense);
        
        context.SaveChanges();
    }
    public IEnumerable<Expense> GetExpensesLoggedInUser(LoggedInUserId loggedInUserId)
    {
        var user = context
            .Users
            .Include(x => x.User!)
            .ThenInclude(user => user.Expenses)
            .Single(x => x.Id == loggedInUserId.Value)
            .User;

        return user?.Expenses ?? [];
    }
}