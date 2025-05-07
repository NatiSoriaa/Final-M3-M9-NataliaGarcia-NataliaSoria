using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;
using UserApi.Models;
using UserBookApi.Models;
using BooksApiCall;
using Microsoft.AspNetCore.Identity;
using IdentityApiAuth.Data;
using LoggedUserAuth.Models; //Crear autentificacion de usuarios


/* CONSTRUIMOS LA APLICACIÓN WEB */

var builder = WebApplication.CreateBuilder(args);


/* CREAMOS LOS SERVICIOS */

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Importamos las conexiones a las BBDD
builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("LibraryConnection")));
builder.Services.AddDbContext<UserContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("UserConnection")));
builder.Services.AddDbContext<UserBookContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("UserBookConnection")));
builder.Services.AddDbContext<UserContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));

// Configuración de Autenticación y Autorización
builder.Services
    .AddAuthorization()
    .AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

// Configuración de Identity
builder.Services
    .AddIdentityCore<UserContext>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserContext>()
    .AddApiEndpoints();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/* LLAMAMOS A LA API LIBROS*/

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
            newBook.Urlcover = $"https://covers.openlibrary.org/b/id/{book.CoverID}-L.jpg";
        
            books.Add(newBook);
        }
    }  
}


/* LEVANTAMOS LA APLICACIÓN */


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//construimos el middleware
app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Definimos los endpointss y la autorización
app.MapGet("/home", (HttpContext httpContext) =>
{
    return new
    {
        httpContext.User.Identity.Name,
        httpContext.User.Identity.AuthenticationType,
        Claims = httpContext.User.Claims.Select(s => new
        {
            s.Type, s.Value
        }).ToList()
    };
})
.RequireAuthorization();

//levantamos la aplicacion
app.Run();

