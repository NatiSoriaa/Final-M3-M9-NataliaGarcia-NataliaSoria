public class BooksApiResponse
{
    public List<BooksApi> Docs { get; set; } = new List<BooksApi>();
}

public class BooksApi
{
    public string Title { get; set; } = string.Empty; 
    public List<string> Author_Name { get; set; } = new List<string>(); 
    public int? CoverID { get; set; } // "cover_i"
    public int? First_Publish_Year { get; set; } 
}