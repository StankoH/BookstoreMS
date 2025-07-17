namespace BookstoreMS.Models.DTOs
{
    public class BookImportDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public List<string> Authors { get; set; } = new();
        public List<string> Genres { get; set; } = new();
    }
}