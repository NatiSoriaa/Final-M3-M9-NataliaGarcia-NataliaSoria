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

    // RECUPERAR LOS ULTIMOS 10 LIBROS AÑADIDOS A LA BBDD
    [HttpGet("lastbooks")]

    //DEVOLVEMOS TODOS LOS LIBROS DE NUESTRA BBDD
    public async Task<ActionResult<IEnumerable<LibraryItem>>> GetLastBooks()
    {
        
        return await _context.LibraryItems
        .OrderByDescending(b => b.DateTime)  
        .Take(10)
        .ToListAsync();
    }


    // RECUPERAR LIBRO POR ID
    [HttpGet("GetOneBooks/{book_id}")]

    //DEVOLVEMOS TODOS LOS LIBROS DE NUESTRA BBDD
    public async Task<ActionResult<LibraryItem>> GetOneBooks(int book_id)
    {
        var book = await _context.LibraryItems.FindAsync(book_id);
         if (book == null)
        {
            return NotFound();
        }
        return book;
    }

}

