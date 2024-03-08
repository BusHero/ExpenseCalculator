namespace ExpensesManager.DataAccess;

public class Expense2
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;
    
    public decimal Amount { get; set; }
}
