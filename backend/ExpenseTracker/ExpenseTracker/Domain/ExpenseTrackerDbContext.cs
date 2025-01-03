using ExpenseTracker.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Domain;

public class ExpenseTrackerDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the decimal property for Amount
        modelBuilder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasPrecision(18, 2); // Precision of 18, scale of 2
        
        // Configure DateTime properties to use DateTime2
        modelBuilder.Entity<Expense>()
            .Property(e => e.ExpenseDate)
            .HasColumnType("DateTime");

        modelBuilder.Entity<Expense>()
            .Property(e => e.UpdatedAt)
            .HasColumnType("DateTime");
    }
}