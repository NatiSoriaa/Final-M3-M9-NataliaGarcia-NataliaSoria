using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;
using UserApi.Models;
using UserBookApi.Models;
using BooksApiCall;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await MainAsync();

async Task MainAsync()
{

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
    builder.Services.AddHttpClient<BooksApiCall.BooksApiCall>();
    builder.Services.AddScoped<LibraryApi.Services.LibraryItemService>();


    // Configuración de Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Library API",
            Version = "v1"
        });
    });


    /* LLAMAMOS A LA API LIBROS*/

    //llamamos a la API de libros 
    builder.Services.AddHttpClient<BooksApiCall.BooksApiCall>();

    async Task<List<BooksApiProduct>> GetBooksAsync(IServiceProvider services)
    {
        var booksApiCall = services.GetRequiredService<BooksApiCall.BooksApiCall>();
        return await booksApiCall.GetBooksAsync();
    }

    // Configurar servicios de CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    //Revisamos los libros recibidos por la API
    using (var scope = app.Services.CreateScope())
    {
        Console.WriteLine("🟢 Llamando a la API para obtener libros...");
        var services = scope.ServiceProvider;

        //inicializamos los contextos 
        var booksApiCall = services.GetRequiredService<BooksApiCall.BooksApiCall>();
        var LibraryServices = services.GetRequiredService<LibraryApi.Services.LibraryItemService>();

        var bookList = await booksApiCall.GetBooksAsync();
        Console.WriteLine($"🔎 Se obtuvieron {bookList.Count} libros de la API.");

        foreach (var book in bookList)
        {
            //revisamos si el libro ya existe en la BBDD
            var existsBook = await LibraryServices.CheckIfExists(book.Title, book.Author);

            LibraryItem newBook = new LibraryItem();
            //Si no encuentra el libro en nuestra BBDD lo añadimos nuevo
            if(existsBook ==null)
            {
                newBook.Title = book.Title ?? string.Empty;
                newBook.Author = book.Author ?? string.Empty;
                newBook.PublishedDate = book.PublishedDate == 0 ? 0 : book.PublishedDate;

                //llamamos a la API portadas de libros para que nos devuelva la url de la portada
                newBook.Urlcover = $"https://covers.openlibrary.org/b/id/{book.CoverID}-L.jpg";
                Console.WriteLine($"✅ Libro añadido: {book.Title} - {book.Author}");

                await LibraryServices.AddBook(newBook.Title, newBook.Author, newBook.Urlcover, newBook.PublishedDate); 
            }
        }  
    }


    /* LEVANTAMOS LA APLICACIÓN */

    // Usar la política de CORS
    app.UseCors("AllowAllOrigins");
   

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        });
    }

    //construimos el middleware

    app.UseHttpsRedirection();


    app.MapControllers();

    app.UseDeveloperExceptionPage();

    app.UseStaticFiles();
  

    //Definimos los endpointss y la autorización
    app.MapGet("/login",async context=>
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "login-register.html");
        await context.Response.SendFileAsync(filePath);
    });
     app.MapGet("/home",async context=>
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "index.html");
        await context.Response.SendFileAsync(filePath);
    });


    //levantamos la aplicacion
    app.Run();
}

