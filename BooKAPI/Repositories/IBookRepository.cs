using BookAPI.Models;

namespace BookAPI.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooks();
        Task<Book?> GetBookById(int id);
        Task<Book?> GetBookByTitle(string title);
        Task AddBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(int id);
    }
}
