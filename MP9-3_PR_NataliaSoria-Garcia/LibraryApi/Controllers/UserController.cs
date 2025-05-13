
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Models;
using Microsoft.AspNetCore.Authorization;

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

        [AllowAnonymous] 
        //BUSCAR USUARIO POR NICKNAME (Funcion para cuando loggeamos, recibir password e id)
        [HttpGet("GetUserByNickname")]
        public async Task<ActionResult> GetUserByNickname([FromQuery] string nickname, [FromQuery] string password)
        {
            
            try {
                var userExists = await _context.UserItems.FirstOrDefaultAsync(x => x.Nickname == nickname);

                if (userExists == null || !BCrypt.Net.BCrypt.Verify(password, userExists.Password))
                {
                    return Unauthorized("Credenciales incorrectas.");
                }

                return Ok(new { message = "Login correcto", id = userExists.Id});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }
     
        [AllowAnonymous] 
        // REGRISTRAR UN NUEVO USUARIO
        [HttpPost]
        public async Task<ActionResult> PostUserItem(UserItem userItem)
        {
            //Encriptar la contraseña
            userItem.Password = BCrypt.Net.BCrypt.HashPassword(userItem.Password);
            
            _context.UserItems.Add(userItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Nuevo usuario creado", id = userItem.Id, userItem = userItem});
        }

        [AllowAnonymous] 
        // PUT: api/User/
        //Actualizar la info del usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem(int id, UserItem userItem)
        {
            //confirmamso que el id del usuario activo en la sesion sea el mismo del usuario que vamos a modificar
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

        [AllowAnonymous] 
        //FUNCION PARA CONTRASEÑA OLVIDADA
        public class EmailRequest
        {
            public string Email { get; set; }
        }

        [HttpPost("passwordForgot")]
        public async Task<IActionResult> ResetPAssword([FromBody] EmailRequest request)
        {
            var email = request.Email;
            var user = await _context.UserItems.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("Usuario no encontrado con ese email.");
            }

            // Generar nueva contraseña aleatoria
            string newPassword = RandomPassword();
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            // Devolvemos por consola la nueva contraseña
            return Ok(new { message = "Contraseña restablecida", nuevaPassword = newPassword});
        }

        [AllowAnonymous] 
        //FUncion para crear contraseña aleatoria
        private string RandomPassword()
        {
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz123456789";
            var random = new Random();

            //INTENTE AsÍ PERO ME DABA ERROR
            // string newPaswword="";
            // for(int i=0; i < 8; i++)
            // {
            //     int index = random.Next(caracteres.Length);
            //     newPaswword.ToArray()[i] = caracteres[index];
            // }
            
            // return newPaswword;

            //solucion que me dio ChatGPT
            return new string(Enumerable.Repeat(caracteres, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}


