namespace BookstoreMS.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = null!;
    public int AuthorYearOfBirth { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}