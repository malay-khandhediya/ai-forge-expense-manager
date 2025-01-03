using ExpenseTracker.Model;

namespace ExpenseTracker.Domain.Model;

public class Expense
{
    public int ExpenseId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Category Category { get; set; } = null!;
}