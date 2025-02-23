using BookAPI.Data;
using BookAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly BookAPIContext _context;
        public BooksController(BookAPIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of books, sorted by popularity.
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of books per page (default: 10)</param>
        /// <returns>Paginated book list</returns>
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { message = "Page and pageSize must be greater than zero." });
            }

            var currentYear = DateTime.Now.Year;

            var booksQuery = await _context.Books
                .Where(b => !b.IsDeleted)
                .ToListAsync();

            var books = booksQuery
                .Select(b => new
                {
                    b.Title,
                    PopularityScore = (b.Views * 0.5) + ((currentYear - b.PublicationYear) * 2)
                })
                .OrderByDescending(b => b.PopularityScore)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new { b.Title })
                .ToList();

            var totalBooks = booksQuery.Count;

            var response = new
            {
                TotalBooks = totalBooks,
                TotalPages = (int)Math.Ceiling((double)totalBooks / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                Books = books
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieves details of a specific book by ID.
        /// </summary>
        /// <param name="id">The ID of the book</param>
        /// <returns>Book details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (book == null)
            {
                return NotFound(new { message = "Book not found." });
            }

            book.Views++;
            await _context.SaveChangesAsync();

            return Ok(book);
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">Book object to add</param>
        /// <returns>The created book</returns>
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (_context.Books.Any(b => b.Title == book.Title))
            {
                return Conflict(new { message = "A book with the same title already exists" });
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        /// <summary>
        /// bulk Adds new books to the database.
        /// </summary>
        /// <param name="books">Book objects to add</param>
        /// <returns>created books</returns>
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> books)
        {
            foreach (var book in books)
            {
                if (_context.Books.Any(b => b.Title == book.Title))
                {
                    return Conflict(new { message = $"A book with the title '{book.Title}' already exists." });
                }
            }

            _context.Books.AddRange(books);
            await _context.SaveChangesAsync();

            return Created("Books added successfully", books);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">ID of the book to update</param>
        /// <param name="updatedBook">Updated book object</param>
        /// <returns>Updated book</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new { message = "Book not found." });
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.PublicationYear = updatedBook.PublicationYear;

            await _context.SaveChangesAsync();

            return Ok(book);
        }

        /// <summary>
        /// Soft deletes a book by ID.
        /// </summary>
        /// <param name="id">ID of the book to soft delete</param>
        /// <returns>Status message</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new { message = "Book not found." });
            }

            book.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Bulk deletes books based on their IDs.
        /// </summary>
        /// <param name="bookIds">List of book IDs to delete</param>
        /// <returns>Status message</returns>
        [HttpDelete("bulk")]
        public async Task<IActionResult> SoftDeleteBooks([FromBody] List<int> bookIds)
        {
            var books = _context.Books.Where(b => bookIds.Contains(b.Id)).ToList();

            if (books.Count == 0)
            {
                return NotFound(new { message = "No books found for deletion." });
            }

            books.ForEach(b => b.IsDeleted = true);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
