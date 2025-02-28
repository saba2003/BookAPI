using BookAPI.Data;
using BookAPI.DTOs;
using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Retrieves a paginated list of books, sorted by popularity.
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of books per page (default: 10)</param>
        /// <returns>Paginated book list</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest(new { message = "Page and pageSize must be greater than zero." });

            var books = await _bookService.GetBooksByPopularity(page, pageSize);
            return Ok(books);
        }

        /// <summary>
        /// Retrieves details of a specific book by ID.
        /// </summary>
        /// <param name="id">The ID of the book</param>
        /// <returns>Book details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound(new { message = "Book not found." });
            }

            return Ok(book);
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="bookDto">Book object to add</param>
        /// <returns>The created book</returns>
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBook = await _bookService.AddBook(bookDto);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }

        /// <summary>
        /// bulk Adds new books to the database.
        /// </summary>
        /// <param name="bookDtos">Book objects to add</param>
        /// <returns>created books</returns>
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooks([FromBody] List<BookDto> bookDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBooks = await _bookService.AddBooks(bookDtos);
            return Created("Books added successfully", createdBooks);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">ID of the book to update</param>
        /// <param name="updatedBookDto">Updated book object</param>
        /// <returns>Updated book</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto updatedBookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedBook = await _bookService.UpdateBook(id, updatedBookDto);
            if (updatedBook == null)
            {
                return NotFound(new { message = "Book not found." });
            }

            return Ok(updatedBook);
        }

        /// <summary>
        /// Soft deletes a book by ID.
        /// </summary>
        /// <param name="id">ID of the book to soft delete</param>
        /// <returns>Status message</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteBook(int id)
        {
            var result = await _bookService.SoftDeleteBook(id);
            if (!result)
            {
                return NotFound(new { message = "Book not found." });
            }

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
            var result = await _bookService.SoftDeleteBooks(bookIds);
            if (!result)
            {
                return NotFound(new { message = "No books found for deletion." });
            }

            return NoContent();
        }
    }
}
