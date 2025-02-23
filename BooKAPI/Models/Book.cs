using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Author { get; set; }

        public int PublicationYear { get; set; }

        public int Views { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;
    }
}
