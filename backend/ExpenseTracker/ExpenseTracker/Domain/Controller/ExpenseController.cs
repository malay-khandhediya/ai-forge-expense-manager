using ExpenseTracker.Model;
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
        return await _context.Expenses.Include(e => e.Category).ToListAsync();
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
        _context.Entry(expense).State = EntityState.Modified;
        expense.UpdatedAt = DateTime.UtcNow;
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
}