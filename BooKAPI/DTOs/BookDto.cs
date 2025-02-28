namespace BookAPI.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Author { get; set; }
        public int PublicationYear { get; set; }
        public int Views { get; set; }
    }
}
