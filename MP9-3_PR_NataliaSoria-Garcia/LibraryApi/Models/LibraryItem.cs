namespace LibraryApi.Models;

public class LibraryItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Urlcover { get; set; } = string.Empty;
    public int PublishedDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int Puntuation { get; set; } = int.MinValue;
}
