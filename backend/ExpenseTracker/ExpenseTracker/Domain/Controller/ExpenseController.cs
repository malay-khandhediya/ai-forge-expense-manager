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
}