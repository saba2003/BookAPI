using BookAPI.DTOs;
using BookAPI.Models;

namespace BookAPI.Mappers
{
    public static class BookMapper
    {
        public static BookDto ToDto(this Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublicationYear = book.PublicationYear,
                Views = book.Views
            };
        }

        public static Book ToModel(this BookDto bookDto)
        {
            return new Book
            {
                Id = bookDto.Id,
                Title = bookDto.Title,
                Author = bookDto.Author,
                PublicationYear = bookDto.PublicationYear,
                Views = bookDto.Views
            };
        }
    }
}
