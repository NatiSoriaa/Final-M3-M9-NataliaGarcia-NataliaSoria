using System.Reflection.Metadata;

namespace UserBookApi.Models;

public class UserBookItem
{  
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Comment { get; set; } = null;
    public int Rating { get; set; } = 0;
}