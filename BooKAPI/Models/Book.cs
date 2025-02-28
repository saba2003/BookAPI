using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models
{
    public class Book
    {
        [Key]
        private int BookId;

        private string BookTitle = string.Empty;

        private string? BookAuthor;

        private int BookPublicationYear;

        private int BookViews = 0;

        private bool IsBookDeleted = false;

        public int Id
        {
            get { return BookId; }
            set { BookId = value; }
        }
        public string Title
        {
            get { return BookTitle; }
            set { BookTitle = value; }
        }
        public string? Author
        {
            get { return BookAuthor; }
            set { BookAuthor = value; }
        }
        public int PublicationYear
        {
            get { return BookPublicationYear; }
            set { BookPublicationYear = value; }
        }
        public int Views
        {
            get { return BookViews; }
            set { BookViews = value; }
        }
        public bool IsDeleted
        {
            get { return IsBookDeleted; }
            set { IsBookDeleted = value; }
        }
    }
}
