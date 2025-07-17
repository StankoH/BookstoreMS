namespace BookstoreMS.Entities;

public class Review
{
    public int ReviewId { get; set; }
    public int ReviewRating { get; set; }
    public string ReviewDescription { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}