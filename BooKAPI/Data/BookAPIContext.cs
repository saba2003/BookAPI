using Microsoft.EntityFrameworkCore;
using BookAPI.Models;

namespace BookAPI.Data
{
    public class BookAPIContext : DbContext
    {
        public BookAPIContext(DbContextOptions<BookAPIContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1,  Title = "Design patterns",                 Author = "Walter",               PublicationYear = 2001, Views = 9 },
                new Book { Id = 2,  Title = "Clean Code",                      Author = "Robert C. Martin",     PublicationYear = 2008, Views = 15 },
                new Book { Id = 3,  Title = "The Pragmatic Programmer",        Author = "Andrew Hunt",          PublicationYear = 1999, Views = 13 },
                new Book { Id = 4,  Title = "Refactoring",                     Author = "Martin Fowler",        PublicationYear = 2018, Views = 2 },
                new Book { Id = 5,  Title = "Code Complete",                   Author = "Steve McConnell",      PublicationYear = 2004, Views = 16 },
                new Book { Id = 6,  Title = "Introduction to Algorithms",      Author = "Thomas H. Cormen",     PublicationYear = 2009, Views = 10 },
                new Book { Id = 7,  Title = "You Don't Know JS",               Author = "Kyle Simpson",         PublicationYear = 2015, Views = 4 },
                new Book { Id = 8,  Title = "Eloquent JavaScript",             Author = "Marijn Haverbeke",     PublicationYear = 2011, Views = 12 },
                new Book { Id = 9,  Title = "Python Crash Course",             Author = "Eric Matthes",         PublicationYear = 2015, Views = 1 },
                new Book { Id = 10, Title = "The Art of Computer Programming", Author = "Donald Knuth",         PublicationYear = 1968 },
                new Book { Id = 11, Title = "Java: The Complete Reference",    Author = "Herbert Schildt",      PublicationYear = 2014 },
                new Book { Id = 12, Title = "C# in Depth",                     Author = "Jon Skeet",            PublicationYear = 2019 },
                new Book { Id = 13, Title = "Software Engineering",            Author = "Ian Sommerville",      PublicationYear = 2015 },
                new Book { Id = 14, Title = "Database System Concepts",        Author = "Abraham Silberschatz", PublicationYear = 2011 },
                new Book { Id = 15, Title = "Head First Design Patterns",      Author = "Eric Freeman",         PublicationYear = 2004 }
            );
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Book> Books { get; set; }
    }
}
