using Microsoft.EntityFrameworkCore; 
namespace UserBookApi.Models;
public class UserBookContext : DbContext
{
    public UserBookContext(DbContextOptions<UserBookContext> options) : base (options) {}
    public DbSet<UserBookItem> UserBookItems {get; set;} = null!;
}