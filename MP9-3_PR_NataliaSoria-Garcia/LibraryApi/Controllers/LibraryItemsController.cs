using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;

namespace LibraryApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryItemsController : ControllerBase
{
    private readonly LibraryContext _context;

    public LibraryItemsController(LibraryContext context)
    {
        _context = context;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LibraryItem>>> GetLibraryItems()
    {
        return await _context.LibraryItems.ToListAsync();
    }


    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLibraryItem(long id, LibraryItem libraryItem)
    {
        if (id != libraryItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(libraryItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<LibraryItem>> PostTodoItem(LibraryItem libraryItem)
    {
        _context.LibraryItems.Add(libraryItem);
        await _context.SaveChangesAsync();

        // return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        return CreatedAtAction(nameof(GetLibraryItems), new { id = libraryItem.Id }, libraryItem);
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.LibraryItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.LibraryItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return _context.LibraryItems.Any(e => e.Id == id);
    }
}

