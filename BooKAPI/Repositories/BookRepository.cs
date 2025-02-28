using BookAPI.Data;
using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookAPIContext _context;

        public BookRepository(BookAPIContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetBooks()
        {
            return await _context.Books.Where(b => !b.IsDeleted).ToListAsync();
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<Book?> GetBookByTitle(string title)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Title == title && !b.IsDeleted);
        }

        public async Task AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBook(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(int id)
        {
            var book = await GetBookById(id);
            if (book != null)
            {
                book.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
