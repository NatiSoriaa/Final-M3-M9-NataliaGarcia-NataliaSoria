using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        //BUSCAR USUARIO POR NICKNAME (Funcion para cuando loggeamos, recibir password e id)
        [HttpGet("nickname/{nickname}")]
        public async Task<ActionResult<UserItem>> GetUserByNickname(string nickname)
        {
            var userExists = await _context.UserItems.Where(x => x.Nickname == nickname).FirstOrDefaultAsync();

            if (userExists == null)
            {
                return NotFound();
            }

            return userExists;
        }

        //BUSCAR USUARIO POR NICKNAME (Funcion para cuando loggeamos, recibir password e id)
        [HttpGet("id/{id}")]
        public async Task<ActionResult<string>> GetUserNicknameById(int id)
        {
            var userExists = await _context.UserItems.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (userExists == null)
            {
                return NotFound();
            }

            return userExists.Nickname;
        }
      
      

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserItem>> PostUserItem(UserItem userItem)
        {
            _context.UserItems.Add(userItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserNicknameById", new { id = userItem.Id }, userItem);
        }

        // PUT: api/User/
        //Actualizar la info del usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem(int id, UserItem userItem)
        {
            if (id != userItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(userItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            
                return NotFound();

            }

            return NoContent();
        }
       
    }
}
