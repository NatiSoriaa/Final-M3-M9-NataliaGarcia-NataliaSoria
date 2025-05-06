
using System.Text.Json;

namespace BooksApiCall
{
    public class BooksApiCall
    {
        public BooksApiCall(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetBooksAsync()
        {
            var response = await _httpClient.GetAsync($"https://openlibrary.org/search.json?q=crime+and+punishment&fields=key,title,author_name,editions");
            response.EnsureSuccessStatusCode();

            var responseJason = await response.Content.ReadAsStringAsync();
            var books = JsonSerializer.Deserialize<BooksApiProduct>(responseJason);
            if (books == null)
            {
                throw new Exception("No se ha podido recuperar la información de los libros en la API");
            }
            return books;

        }
    }
}