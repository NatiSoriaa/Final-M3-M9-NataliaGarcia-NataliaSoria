using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

    //DEVOLVEMOS TODOS LOS LIBROS DE NUESTRA BBDD
    public async Task<ActionResult<IEnumerable<LibraryItem>>> GetLibraryItems()
    {
        
        return await _context.LibraryItems.ToListAsync();
    }



    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    //POSTEAMOS UN NUEVO LIBRO A NUESTRA BBDD
    public async Task<ActionResult<LibraryItem>> PostLibraryItem( string title, string author, string urlcover, int publishedDate)
    {
        LibraryItem libraryItem = new LibraryItem
        {
            Title = title,
            Author = author,
            Urlcover = urlcover,
            PublishedDate = publishedDate,
            Puntuation = 0,
            DateTime = DateTime.Now
        };
        
        _context.LibraryItems.Add(libraryItem);
        await _context.SaveChangesAsync();

        // return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        return CreatedAtAction(nameof(GetLibraryItems), new { id = libraryItem.Id }, libraryItem);
        
    }


    // [HttpGet("{title,author}")]
    // public async Task<ActionResult<IEnumerable<LibraryItem>>> CheckIfExists(string title,string author)

    // //CHECKEAMOS SI UN LIBRO YA EXISTE EN NUESTRA BBDD
    [HttpGet("check")]
    public async Task<ActionResult<LibraryItem>> CheckIfExists([FromQuery] string title, [FromQuery] string author)
    {
        var book = await _context.LibraryItems.FirstOrDefaultAsync(x => x.Title == title && x.Author == author);
        if (book != null)
        {
            return Ok(book);
        }
        else
        {
            return NotFound();
        }
    }
}

