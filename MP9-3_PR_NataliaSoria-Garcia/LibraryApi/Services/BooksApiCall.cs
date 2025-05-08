
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
            List<BooksApiProduct> books = new List<BooksApiProduct>();
            Console.WriteLine("📡 Llamando a la API de libros...");

            var response = await _httpClient.GetAsync($"https://openlibrary.org/search.json?q=all");
            Console.WriteLine($"📡 Respuesta recibida: {(int)response.StatusCode} {response.ReasonPhrase}");
            
            response.EnsureSuccessStatusCode();

            var responseJason = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"📦 JSON recibido (primeros 500 caracteres): {responseJason.Substring(0, Math.Min(500, responseJason.Length))}");

            var booksApiResponse = JsonSerializer.Deserialize<APIResponse>(responseJason);

            if (booksApiResponse == null)
            {
                throw new Exception("No se ha podido recuperar la información de los libros en la API");
            }

            foreach (var book in booksApiResponse.Docs)
            {
                Console.WriteLine($"📚 Libro: {book.Title} - Autor: {string.Join(", ", book.Author_Name)} - Portada: {book.Cover_I} - Año de publicación: {book.First_Publish_Year}");
            
                books.Add( new BooksApiProduct
                    {Title = book.Title,
                    Author = book.Author_Name[0],
                    CoverID = book.Cover_I,
                    Urlcover = $"https://covers.openlibrary.org/b/id/{book.Cover_I}-L.jpg",
                    PublishedDate = book.First_Publish_Year}
                );
            }
            return books;
        }
    }
    public class APIResponse
    {
        public int NumFound { get; set; } // "numFound"
        public int Start { get; set; } // "start"
        public bool NumFoundExact { get; set; } // "numFoundExact"
        public int NumFoundDocs { get; set; } // "num_found"
        public string DocumentationUrl { get; set; } // "documentation_url"
        public string Query { get; set; } // "q"
        public object Offset { get; set; } // "offset"
        public List<BooksApiDocs> Docs { get; set; } = new List<BooksApiDocs>(); // "docs"
    }
    public class BooksApiDocs
    {
        public string Title { get; set; } = string.Empty; 
        public List<string> Author_Name { get; set; } = new List<string>(); 
        public int Cover_I { get; set; } // "cover_i"
        public int First_Publish_Year { get; set; } 
    }
}

