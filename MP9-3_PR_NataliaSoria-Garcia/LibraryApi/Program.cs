using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;
using UserApi.Models;
using UserBookApi.Models;
using LibraryApi.Controllers;
using BooksApiCall;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Importamos las conexiones a las BBDD
builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("LibraryConnection")));
builder.Services.AddDbContext<UserContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("UserConnection")));
builder.Services.AddDbContext<UserBookContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("UserBookConnection")));

//llamamos a la API de libros 
builder.Services.AddHttpClient<BooksApiCall.BooksApiCall>();

List<LibraryItem> books = new List<LibraryItem>();

async Task<List<BooksApiProduct>> GetBooksAsync(IServiceProvider services)
{
    var booksApiCall = services.GetRequiredService<BooksApiCall.BooksApiCall>();
    return await booksApiCall.GetBooksAsync();
}

var app = builder.Build();

//Revisamos los libros recibidos por la API
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var bookList = await GetBooksAsync(services);

    foreach (var book in bookList)
    {
        //revisamos si el libro ya existe en la BBDD
        async Task<IEnumerable<BooksApiProduct>> CheckIfExists(string title, string author)
        {
            return await CheckIfExists( title, author);
        }
        var existsBook = await CheckIfExists(book.Title, book.Author);

        LibraryItem newBook = new LibraryItem();
        //Si no encuentra el libro en nuestra BBDD lo añadimos nuevo
        if(existsBook ==null)
        {
            newBook.Title = book.Title ?? string.Empty;
            newBook.Author = book.Author ?? string.Empty;
            newBook.PublishedDate = book.PublishedDate == 0 ? 0 : book.PublishedDate;

            //llamamos a la API portadas de libros para que nos devuelva la url de la portada
            newBook.Urlcover = $"https://covers.openlibrary.org/b/id/{book.coverID}-L.jpg";
        
            books.Add(newBook);
        }
    }  
}

    


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

