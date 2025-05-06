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

//Importamos la llamada a la API de libros
builder.Services.AddHttpClient<BooksApiCall>();

async Task<List<BooksApiProduct>> GetBooksAsync(IServiceProvider services)
{
     List<BooksApiProduct> books = new List<BooksApiProduct>();

    var booksList = await BooksApiCall.GetBooksAsync();

    foreach (var book in books)
    {
        BooksApiProduct newBook = new BooksApiProduct();
    
        newBook.Title = book.Title ?? string.Empty;
        newBook.Author = book.Author ?? string.Empty;
        newBook.coverID = book.coverID ?? string.Empty;
        newBook.PublishedDate = book.PublishedDate == 0 ? 0 : book.PublishedDate;

        books.Add(newBook);
    
    }
    
    
    return books;
}

var app = builder.Build();

//llamamos a la APi y guardamos en la BBDD
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LibraryContext>();
    var books = await GetBooksAsync(services);
    
    // Guardamos los libros en la BBDD
    foreach (var book in books)
    {
        string cover = string.Empty;

        LibraryItemsController.PostLibraryItem(book.Author, book.Title,cover, book.PublishedDate).Wait();
    }
    
    context.SaveChanges();
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


public class BooksApiProduct{
    public string Title { get; set; } = string.Empty; //title
    public string Author { get; set; } = string.Empty; // author_name[0]
    public string coverID { get; set; } = string.Empty; //cover_i
    public int PublishedDate { get; set; } //first_publish_year

}