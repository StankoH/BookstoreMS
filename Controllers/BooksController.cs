using BookstoreMS.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookstoreContext _ctx;
    private readonly IConfiguration _cfg;

    public BooksController(BookstoreContext ctx, IConfiguration cfg)
    {
        _ctx = ctx;
        _cfg = cfg;
    }

    [HttpGet]
    [Authorize(Policy = "ReadOnly")]
    public IActionResult GetAllBooks()
    {
        var books = _ctx.Books
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .Include(b => b.Reviews)
            .Take(1000)
            .ToList()
            .Select(b => new BookWithRatingDto(
                b.BookId,
                b.BookTitle,
                b.BookPrice,
                b.BookAuthors?.Select(ba => ba.Author?.AuthorName ?? "").ToList() ?? new List<string>(),
                b.BookGenres?.Select(bg => bg.Genre?.GenreName ?? "").ToList() ?? new List<string>(),
                b.Reviews != null && b.Reviews.Any()
                    ? b.Reviews.Average(r => r.ReviewRating)
                    : 0
            ))
            .OrderBy(b => b.Title)
            .ToList();

        return Ok(books);
    }



    [HttpGet("top10")]
    [Authorize(Policy = "ReadOnly")]
    public IActionResult GetTop10Books()
    {
        var result = new List<BookWithRatingDto>();

        using var conn = new SqlConnection(_cfg.GetConnectionString("DefaultConnection"));
        using var cmd = new SqlCommand("EXEC GetTop10BooksByAvgRating", conn);
        conn.Open();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new BookWithRatingDto(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetDecimal(2),
                new List<string>(),
                new List<string>(),
                reader.IsDBNull(3) ? 0 : Convert.ToDouble(reader.GetDecimal(3))
            ));
        }

        return Ok(result);
    }

    [HttpPut("{id}/price")]
    [Authorize(Policy = "ReadWrite")]
    public IActionResult UpdatePrice(int id, [FromBody] decimal newPrice)
    {
        using var conn = new SqlConnection(_cfg.GetConnectionString("DefaultConnection"));
        using var cmd = new SqlCommand("EXEC UpdateBookPrice @BookId, @NewPrice", conn);
        cmd.Parameters.AddWithValue("@BookId", id);
        cmd.Parameters.AddWithValue("@NewPrice", newPrice);

        conn.Open();
        cmd.ExecuteNonQuery();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ReadWrite")]
    public IActionResult DeleteBook(int id)
    {
        using var conn = new SqlConnection(_cfg.GetConnectionString("DefaultConnection"));
        using var cmd = new SqlCommand("EXEC DeleteBook @BookId", conn);
        cmd.Parameters.AddWithValue("@BookId", id);

        conn.Open();
        cmd.ExecuteNonQuery();

        return NoContent();
    }
}