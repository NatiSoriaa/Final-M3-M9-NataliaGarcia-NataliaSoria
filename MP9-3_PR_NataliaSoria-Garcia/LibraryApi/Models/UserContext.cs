using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LoggedUserAuth.Models;

namespace UserApi.Models;
public class UserContext : IdentityDbContext<UserLogged>
{
    public UserContext(DbContextOptions<UserContext> options) : base (options) {}
    public DbSet<UserItem> UserItems {get; set;} = null!;
}  