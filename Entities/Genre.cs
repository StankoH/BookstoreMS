namespace BookstoreMS.Entities;

public class Genre
{
    public int GenreId { get; set; }
    public string GenreName { get; set; } = null!;

    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}