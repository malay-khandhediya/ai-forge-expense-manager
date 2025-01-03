namespace ExpenseTracker.Domain.Model;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
        
    public ICollection<Expense>? Expenses { get; set; } // Navigation property
}