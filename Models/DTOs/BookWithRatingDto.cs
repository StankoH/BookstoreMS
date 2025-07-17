namespace BookstoreMS.Models.DTOs;

public record BookWithRatingDto(
    int BookId,
    string Title,
    decimal Price,
    IEnumerable<string> Authors,
    IEnumerable<string> Genres,
    double AverageRating);