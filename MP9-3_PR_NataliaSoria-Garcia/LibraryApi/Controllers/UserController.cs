
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
                    return Unauthorized(new { message = "Credenciales incorrectas." });

                }

                return Ok(new { message = "Login correcto", id = userExists.Id});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }

        [AllowAnonymous] 
        //REVIRAR SI EL USUARIO YA EXISTE
        [HttpGet("UserExists")]
        public async Task<ActionResult> UserExists([FromQuery] string nickname, [FromQuery] string email)
        {        
                var userExists = await _context.UserItems.AnyAsync(x => x.Nickname == nickname || x.Email==email);

                return Ok(new {userExists});
        }

        //RECUPERAR USUARIO POR ID
        [HttpGet("GetUserID")]
        public async Task<ActionResult<UserItem>> GetUserID([FromQuery] int id)
        {        
                var userExists = await _context.UserItems.FindAsync(id);
                if(userExists==null) return NotFound();

                return Ok(new {userExists});
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
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> PutUserItem([FromBody] UserItem userItem)
        {
            //confirmamso que el id del usuario activo en la sesion sea el mismo del usuario que vamos a modificar
            if ( userItem ==null)
            {
                return BadRequest();
            }

            var existingUser = await _context.UserItems.FindAsync(userItem.Id);

            if(existingUser==null)
            {
                return NotFound(new {message="usuario no encontrado"});
            }
            //actualizamos la informacion
            if (!string.IsNullOrWhiteSpace(userItem.Nickname) && userItem.Nickname != existingUser.Nickname)
            {
                existingUser.Nickname = userItem.Nickname;
            }
            if (!string.IsNullOrWhiteSpace(userItem.Email) && userItem.Email != existingUser.Email)
            {
                existingUser.Email = userItem.Email;
            }
            if (!string.IsNullOrWhiteSpace(userItem.Password) && !BCrypt.Net.BCrypt.Verify(userItem.Password, existingUser.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userItem.Password);
            }
                      
            
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Usuario actualizado correctamente.", user = existingUser });
            }
            catch (DbUpdateConcurrencyException e)
            {
                 return StatusCode(500, new { message = "Error al guardar los cambios", detail = e.Message });

            }

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


