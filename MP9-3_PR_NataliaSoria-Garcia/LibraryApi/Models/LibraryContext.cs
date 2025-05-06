using Microsoft.EntityFrameworkCore; 
    namespace LibraryApi.Models;
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base (options) {}
        public DbSet<LibraryItem> LibraryItems {get; set;} = null!;
    }  