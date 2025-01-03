using ExpenseTracker.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Domain.Controller;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly ExpenseTrackerDbContext _context;
    public ExpenseController(ExpenseTrackerDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
    {
        return await _context.Expenses.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetExpenseById(int id)
    {
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.ExpenseId == id);

        if (expense == null)
        {
            return NotFound();
        }

        return Ok(expense);
    }
    [HttpPost]
    public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetExpenses), new { id = expense.ExpenseId }, expense);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, Expense expense)
    {
        if (id != expense.ExpenseId)
            return BadRequest();
        if(expense.ExpenseDate == null) return BadRequest("ExpenseDate is required");
        var expenseDb =_context.Expenses.FirstOrDefault(c => c.ExpenseId == id);
        if(expenseDb == null) return NotFound();
        expenseDb.Amount = expense.Amount;
        expenseDb.CategoryId = expense.CategoryId;
        expenseDb.Description = expense.Description;
        expenseDb.ExpenseDate = expense.ExpenseDate;
        expenseDb.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null) return NotFound();
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("summary")]
    public async Task<ActionResult<IEnumerable<CategorySummary>>> GetExpenseSummaryByCategory()
    {
        var summary = await _context.Expenses
            .GroupBy(e => e.Category.Name)
            .Select(g => new CategorySummary
            {
                Category = g.Key,
                TotalAmount = g.Sum(e => e.Amount)
            })
            .ToListAsync();

        return Ok(summary);
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Expense>>> SearchExpenses(string query)
    {
        var expenses = await _context.Expenses
            .Where(e => EF.Functions.Like(e.Description, $"%{query}%") ||
                        EF.Functions.Like(e.Category!.Name, $"%{query}%"))
            .ToListAsync();

        return Ok(expenses);
    }
}