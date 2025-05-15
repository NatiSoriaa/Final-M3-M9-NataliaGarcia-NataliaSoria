using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserBookApi.Models;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookController : ControllerBase
    {
        private readonly UserBookContext _context;
        private readonly LibraryContext _libraryContext;

        public UserBookController(UserBookContext context)
        {
            _context = context;
        }

        // GET: COMENTARIOS DE UN LIBRO
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBookComents(int id_book)
        {
            var userBookItems = await _context.UserBookItems
            .Where(x => x.BookId == id_book && x.Comment != null)
            .Select(x => new
            {
                id = x.Id,
                userId = x.UserId,
                comment = x.Comment,
                rating = x.Rating
            })
            .ToListAsync();
           
            return Ok(userBookItems);

            
        }

        //Encontrar un libro por su id
        [HttpGet("id/{id}")]
        public async Task<ActionResult<UserBookItem>> GetUserBookItem(int id)
        {
            var userBookItem = await _context.UserBookItems.Where(book => book.BookId==id).FirstOrDefaultAsync();

            if (userBookItem == null)
            {
                return NotFound();
            }

            return userBookItem;
        }
    

        // GET: LIBRO POR ESTADO
        [HttpGet("state/{state}")]
        public async Task<ActionResult<object>> GetBookByCategory(string state ,  [FromQuery] int id_user)
        {
            //Comprobamos que el estado no sea nulo
            if (state == null)
            {
                return BadRequest("El estado no puede ser nulo.");
            }

            //Comprobamos que el id del usuario no sea nulo
            if (id_user == 0)
            {
                return BadRequest("El id del usuario no puede ser nulo.");
            }

            //Buscamos los libros por su estado
            {
                var userBookItems = await _context.UserBookItems.Where(x => x.Status == state && x.UserId==id_user).ToListAsync();
                if (userBookItems == null || userBookItems.Count == 0)  return NotFound();
                
                //lista de los id para buscarlos despues en la tabla libro
                var userBooksId = userBookItems.Select(x => x.BookId).ToList();

                //buscamos los libros por su id y los recogemos en una lista
                var userLibraryBooks = await _libraryContext.LibraryItems
                    .Where(x => userBooksId.Contains(x.Id))
                    .ToListAsync();

                return Ok(new {userBookItems, userLibraryBooks});
            }
        }

        // Actualizar el estado del libro
        [HttpPut("{id}")]
        public async Task<ActionResult<UserBookItem>> PutUserBookItem(int id, [FromQuery] string status, [FromQuery] string comment, [FromQuery] int rating)
        {
            var userBookItem = await _context.UserBookItems.FindAsync(id);

            if (userBookItem == null)
            {
                return NotFound();
            }

            //Modificamos el estado en caso de que recibamos valor y este sea diferente al actual
            if(status!="" && status!=userBookItem.Status) userBookItem.Status = status;
            if(comment!="" && comment!=userBookItem.Comment) userBookItem.Comment = comment;
            if(rating!=0 && rating!=userBookItem.Rating) userBookItem.Rating = rating;

            _context.Entry(userBookItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: Añadir un libro para el usuario
        [HttpPost]
        public async Task<ActionResult<UserBookItem>> PostUserBookItem(UserBookItem userBookItem)
        {
            _context.UserBookItems.Add(userBookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserBookItem", new { id = userBookItem.Id }, userBookItem);
        }

        [HttpPut("comment/{comment}")]
        public async Task<ActionResult<UserBookItem>> DeleteComment(int id)
        {
            var userBookItem = await _context.UserBookItems.FindAsync(id);

            if (userBookItem == null)
            {
                return NotFound();
            }

            //Modificamos el comentario a valor nulo.
            userBookItem.Status = "";

            _context.Entry(userBookItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        

        // Borrar  un libro de nuestra Biblioteca
        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteUserBookItem(int id)
        {
            var userBookItem = await _context.UserBookItems.FindAsync(id);
            if (userBookItem == null)
            {
                return NotFound();
            }

            _context.UserBookItems.Remove(userBookItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class Comment
    {
        public int userId { get; set; }
        public string comment { get; set; } = string.Empty;
    }

    public class UserLibraryBooks
    {
        public int book_id { get; set; }
       public string Status { get; set; } = string.Empty;
       public string? Comment { get; set; } = null;
       public int Rating { get; set; } = 0;
    }
}
