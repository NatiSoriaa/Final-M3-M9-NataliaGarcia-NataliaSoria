
using System.Text.Json;
using LibraryApi.Models;
using System.Text.Json.Serialization;


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
            Console.WriteLine($"numFound:" + booksApiResponse.NumFound + " - start:" + booksApiResponse.Start + " - numFoundExact:" + booksApiResponse.NumFoundExact + " - numFoundDocs:" + booksApiResponse.NumFoundDocs + " - documentation_url:" + booksApiResponse.DocumentationUrl + " - q:" + booksApiResponse.Query + " - offset:" + booksApiResponse.Offset);

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
        [JsonPropertyName("numFound")]
        public int NumFound { get; set; } 

         [JsonPropertyName("start")]
        public int Start { get; set; } 

         [JsonPropertyName("numFoundExact")]
        public bool NumFoundExact { get; set; } 

         [JsonPropertyName("num_found")]
        public int NumFoundDocs { get; set; } 

         [JsonPropertyName("documentation_url")]
        public string DocumentationUrl { get; set; } 

         [JsonPropertyName("q")]
        public string Query { get; set; } 

         [JsonPropertyName("offset")]
        public object Offset { get; set; } 

         [JsonPropertyName("docs")]
        public List<BooksApiDocs> Docs { get; set; } = new List<BooksApiDocs>(); 
    }
    public class BooksApiDocs
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty; 

        [JsonPropertyName("author_name")]
        public List<string> Author_Name { get; set; } = new List<string>(); 

        [JsonPropertyName("cover_i")]
        public int Cover_I { get; set; } // "cover_i"

        [JsonPropertyName("first_publish_year")]
        public int First_Publish_Year { get; set; } 
    }
}

