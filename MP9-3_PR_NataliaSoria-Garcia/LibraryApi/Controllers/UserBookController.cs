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

        // GET: api/UserBook
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBookItem>>> GetUserBookItems()
        {
            return await _context.UserBookItems.ToListAsync();
        }

        // GET: api/UserBook/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserBookItem>> GetUserBookItem(int id)
        {
            var userBookItem = await _context.UserBookItems.FindAsync(id);

            if (userBookItem == null)
            {
                return NotFound();
            }

            return userBookItem;
        }

        // PUT: api/UserBook/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserBookItem(int id, UserBookItem userBookItem)
        {
            if (id != userBookItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(userBookItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBookItemExists(id))
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

        // POST: api/UserBook
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserBookItem>> PostUserBookItem(UserBookItem userBookItem)
        {
            _context.UserBookItems.Add(userBookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserBookItem", new { id = userBookItem.Id }, userBookItem);
        }

        // DELETE: api/UserBook/5
        [HttpDelete("{id}")]
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

        private bool UserBookItemExists(int id)
        {
            return _context.UserBookItems.Any(e => e.Id == id);
        }
    }
}
