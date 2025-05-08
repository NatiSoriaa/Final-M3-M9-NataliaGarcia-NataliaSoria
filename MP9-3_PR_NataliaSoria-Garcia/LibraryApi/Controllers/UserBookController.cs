using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserBookApi.Models;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookController : ControllerBase
    {
        private readonly UserBookContext _context;

        public UserBookController(UserBookContext context)
        {
            _context = context;
        }

        // GET: COMENTARIOS DE UN LIBRO
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetBookComents(int id_book)
        {
            List<UserBookItem> userBookItem = await _context.UserBookItems.Where(x => x.BookId == id_book).ToListAsync();
            List<Comment> ListComents= new List<Comment>();
            foreach(UserBookItem item in userBookItem)
            {
                if (item.Comment != null)
                {
                    
                    ListComents.Add(new Comment { userId = item.UserId, comment = item.Comment });
                }
            }
            return ListComents;

            
        }

        //Encontrar un libro por su id
        [HttpGet("id/{id}")]
        public async Task<ActionResult<UserBookItem>> GetUserBookItem(int id)
        {
            var userBookItem = await _context.UserBookItems.FindAsync(id);

            if (userBookItem == null)
            {
                return NotFound();
            }

            return userBookItem;
        }
    

        // GET: LIBRO POR ESTADO
        [HttpGet("state/{state}")]
        public async Task<ActionResult<List<UserLibraryBooks>>> GetBookByCategory(string state)
        {
            List<UserBookItem> userBookItem = await _context.UserBookItems.Where(x => x.Status == state).ToListAsync();

            List<UserLibraryBooks> userLibraryBooks = new List<UserLibraryBooks>();
            foreach (UserBookItem item in userBookItem)
            {
                if (item.Status == state)
                {
                    userLibraryBooks.Add(new UserLibraryBooks { book_id = item.BookId, Status = item.Status, Comment = item.Comment, Rating = item.Rating });
                }
            }

            if (userBookItem == null)
            {
                return NotFound();
            }

            return userLibraryBooks;
        }

        // Actualizar el estado del libro
        [HttpPut("{status,comment,rating}")]
        public async Task<ActionResult<UserBookItem>> PutUserBookItem(int id, string status, string comment, int rating)
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
