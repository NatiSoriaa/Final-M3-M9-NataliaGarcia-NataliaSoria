using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services
{
    public class LibraryItemService
    {
        private readonly LibraryContext _context;

        public LibraryItemService(LibraryContext context)
        {
            _context = context;
        }

        // //CHECKEAMOS SI UN LIBRO YA EXISTE EN NUESTRA BBDD
        public async Task<LibraryItem?> CheckIfExists( string title,  string author)
        {
            var book = await _context.LibraryItems.FirstOrDefaultAsync(x => x.Title == title && x.Author == author);
                if (book != null)
                {
                    return book;
                }
                else
                {
                    return null;
                }
        }

        //POSTEAMOS UN NUEVO LIBRO A NUESTRA BBDD
        public async Task AddBook(string title, string author, string urlcover, int publishedDate)
        {
            LibraryItem libraryItem = new LibraryItem
            {
                Title = title,
                Author = author,
                Urlcover = urlcover,
                PublishedDate = publishedDate,
                Puntuation = 0,
                DateTime = DateTime.Now
            };

            _context.LibraryItems.Add(libraryItem);
            await _context.SaveChangesAsync();
        }
    }
}