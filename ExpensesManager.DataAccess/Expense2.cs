// ReSharper disable PropertyCanBeMadeInitOnly.Global
using System.ComponentModel.DataAnnotations;

namespace ExpensesManager.DataAccess;

public class Expense2
{
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Description { get; set; } = null!;
    
    public required decimal Amount { get; set; }
}
