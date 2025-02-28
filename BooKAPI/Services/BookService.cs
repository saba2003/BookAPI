using BookAPI.DTOs;
using BookAPI.Mappers;
using BookAPI.Models;
using BookAPI.Repositories;

namespace BookAPI.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDto>> GetBooksByPopularity(int page, int pageSize)
        {
            var books = await _bookRepository.GetBooks();
            return books
                .OrderByDescending(b => (b.Views * 0.5) + ((DateTime.Now.Year - b.PublicationYear) * 2))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => b.ToDto())
                .ToList();
        }

        public async Task<BookDto?> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
                return null;

            book.Views++;
            await _bookRepository.UpdateBook(book);

            return book.ToDto();
        }
        public async Task<BookDto> AddBook(BookDto bookDto)
        {
            var existingBook = await _bookRepository.GetBookByTitle(bookDto.Title);
            if (existingBook != null)
                throw new Exception($"A book with the title '{bookDto.Title}' already exists.");

            var book = bookDto.ToModel();
            await _bookRepository.AddBook(book);
            return book.ToDto();
        }
        public async Task<List<BookDto>> AddBooks(List<BookDto> bookDtos)
        {
            var existingBooks = await _bookRepository.GetBooks();
            var newBooks = new List<Book>();
            
            foreach (var bookDto in bookDtos)
            {
                if (existingBooks.Any(b => b.Title == bookDto.Title))
                    throw new Exception($"A book with the title '{bookDto.Title}' already exists.");
                
                newBooks.Add(bookDto.ToModel());
            }

            foreach (var book in newBooks)
            {
                await _bookRepository.AddBook(book);
            }
            return newBooks.Select(b => b.ToDto()).ToList();
        }

        public async Task<BookDto?> UpdateBook(int id, BookDto updatedBookDto)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null) return null;

            book.Title = updatedBookDto.Title;
            book.Author = updatedBookDto.Author;
            book.PublicationYear = updatedBookDto.PublicationYear;

            await _bookRepository.UpdateBook(book);
            return book.ToDto();
        }

        public async Task<bool> SoftDeleteBook(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if(book == null) return false;

            await _bookRepository.DeleteBook(id);
            return true;
        }

        public async Task<bool> SoftDeleteBooks(List<int> bookIds)
        {
            var books = await _bookRepository.GetBooks();
            var booksToDelete = books.Where(b => bookIds.Contains(b.Id)).ToList();

            if(booksToDelete.Count == 0) return false;

            foreach (var book in booksToDelete)
            {
                await _bookRepository.DeleteBook(book.Id);
            }

            return true;
        }
    }
}