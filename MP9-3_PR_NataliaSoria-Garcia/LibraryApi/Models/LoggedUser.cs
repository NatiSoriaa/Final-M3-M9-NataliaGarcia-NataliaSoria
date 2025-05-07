using Microsoft.AspNetCore.Identity;

namespace LoggedUserAuth.Models;

public class UserItem : IdentityUser
{
    public string? Nickname { get; set; }
    public string? Password { get; set; }
}