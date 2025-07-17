using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreMS.Entities;

[Table("Book")]
public class Book
{
    public int BookId { get; set; }
    public string BookTitle { get; set; } = null!;
    public decimal BookPrice { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}