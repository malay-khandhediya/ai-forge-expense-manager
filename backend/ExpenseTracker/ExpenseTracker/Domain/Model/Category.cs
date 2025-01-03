﻿using ExpenseTracker.Domain.Model;

namespace ExpenseTracker.Model;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}