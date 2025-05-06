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



    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<LibraryItem>> PostLibraryItem( string title, string author, string urlcover, int publishedDate)
    {
        LibraryItem libraryItem = new LibraryItem
        {
            Title = title,
            Author = author,
            Urlcover = urlcover,
            PublishedDate = publishedDate,
        };
    {
        _context.LibraryItems.Add(libraryItem);
        await _context.SaveChangesAsync();

        // return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        return CreatedAtAction(nameof(GetLibraryItems), new { id = libraryItem.Id }, libraryItem);
    }
}



    private bool TodoItemExists(long id)
    {
        return _context.LibraryItems.Any(e => e.Id == id);
    }
}

