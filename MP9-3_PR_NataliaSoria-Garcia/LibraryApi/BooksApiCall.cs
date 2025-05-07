
using System.Text.Json;
using LibraryApi.Models;


namespace BooksApiCall
{
    public class BooksApiCall
    {
        private readonly HttpClient _httpClient;
        public BooksApiCall(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Metodo para llamar a la API y devolver los libros en una lista 
        public async Task<List<BooksApiProduct>> GetBooksAsync()
        {
            var response = await _httpClient.GetAsync($"https://openlibrary.org/search.json?q=all");
            response.EnsureSuccessStatusCode();

            var responseJason = await response.Content.ReadAsStringAsync();
            var booksApiResponse = JsonSerializer.Deserialize<BooksApiResponse>(responseJason);
            if (booksApiResponse == null)
            {
                throw new Exception("No se ha podido recuperar la información de los libros en la API");
            }
            var books = booksApiResponse.Docs.Select(book => new BooksApiProduct
            {
                Title = book.Title,
                Author = book.Author,
                Urlcover = $"https://covers.openlibrary.org/b/id/{book.CoverID}-L.jpg",
                PublishedDate = book.PublishedDate,
            }).ToList();

            return books;

        }
    }

    public class BooksApiResponse
    {
        public List<BooksApiProduct> Docs { get; set; } = new List<BooksApiProduct>();
    }

    public class BooksApiProduct{
    public string Title { get; set; } = string.Empty; //title
    public string Author { get; set; } = string.Empty; // author_name[0]
    public int CoverID { get; set; } // cover_i
    public string Urlcover { get; set; } = string.Empty; //cover_url
    public int PublishedDate { get; set; } //first_publish_year
    }

}