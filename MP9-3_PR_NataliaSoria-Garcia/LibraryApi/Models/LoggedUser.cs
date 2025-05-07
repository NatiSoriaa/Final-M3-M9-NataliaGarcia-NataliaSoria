using Microsoft.AspNetCore.Identity;

namespace LoggedUserAuth.Models;

public class UserLogged: IdentityUser 
{
    public string? Nickname { get; set; }
    public string? Password { get; set; }
}