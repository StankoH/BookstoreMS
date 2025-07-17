using BookstoreMS.Helpers;
using BookstoreMS.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace BookstoreMS.Services
{
    public class BookImporterService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BookImporterService> _logger;
        private readonly Random _random = new();

        public BookImporterService(IConfiguration configuration, ILogger<BookImporterService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task ImportBooksFromJsonAsync(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
            {
                _logger.LogError("JSON file not found: {Path}", jsonFilePath);
                return;
            }

            var json = await File.ReadAllTextAsync(jsonFilePath);
            var books = JsonSerializer.Deserialize<List<BookImportDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (books == null || books.Count == 0)
            {
                _logger.LogWarning("No books to import.");
                return;
            }

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            var existingTitles = new List<string>();
            using (var cmd = new SqlCommand("SELECT BookTitle FROM Book", connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    existingTitles.Add(reader.GetString(0).Trim());
                }
            }

            int processed = 0;

            foreach (var dto in books)
            {
                if (string.IsNullOrWhiteSpace(dto.Title)) continue;

                for (int i = 1; i <= 4; i++)
                {
                    string title = $"{dto.Title.Trim()} (Part {i})";

                    bool isDuplicate = existingTitles.Any(existing =>
                        LevenshteinHelper.Calculate(existing.ToLower(), title.ToLower()) <= 2);

                    if (isDuplicate)
                    {
                        _logger.LogWarning("duplikat: {Title}", title);
                        continue;
                    }

                    int bookId = await InsertBookAsync(connection, title, dto.Price);
                    existingTitles.Add(title);

                    foreach (var authorName in dto.Authors.Distinct(StringComparer.OrdinalIgnoreCase))
                    {
                        int authorId = await GetOrInsertAuthorByNameAsync(connection, authorName.Trim());
                        if (authorId > 0)
                        {
                            await InsertBookAuthorAsync(connection, bookId, authorId);
                        }
                    }

                    foreach (var genreName in dto.Genres.Distinct(StringComparer.OrdinalIgnoreCase))
                    {
                        int genreId = await InsertGenreAsync(connection, genreName.Trim());
                        await InsertBookGenreAsync(connection, bookId, genreId);
                    }

                    int reviewCount = _random.Next(1, 4);
                    for (int r = 0; r < reviewCount; r++)
                    {
                        int rating = _random.Next(1, 6);
                        string description = $"Lorem ipsum dolor sit amet, review #{r + 1}";
                        await InsertReviewAsync(connection, bookId, rating, description);
                    }

                    processed++;
                    if (processed % 1000 == 0)
                    {
                        _logger.LogInformation("Inserted {Count} books ", processed);
                    }
                }
            }

            _logger.LogInformation("Import finihsed: {Count} total.", processed);
        }

        private async Task<int> InsertBookAsync(SqlConnection connection, string title, decimal price)
        {
            using var cmd = new SqlCommand("InsertBook", connection) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@BookTitle", title);
            cmd.Parameters.AddWithValue("@BookPrice", price);
            var output = new SqlParameter("@BookId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(output);
            await cmd.ExecuteNonQueryAsync();
            return (int)output.Value;
        }

        private async Task<int> GetOrInsertAuthorByNameAsync(SqlConnection connection, string name)
        {
            using var cmd = new SqlCommand("InsertAuthorByNameOnly", connection) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@AuthorName", name);
            var authorIdParam = new SqlParameter("@AuthorId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(authorIdParam);
            await cmd.ExecuteNonQueryAsync();
            return Convert.ToInt32(authorIdParam.Value);
        }

        private async Task<int> InsertAuthorAsync(SqlConnection connection, string name, int yearOfBirth)
        {
            using var cmd = new SqlCommand("InsertAuthor", connection) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@AuthorName", name.Trim());
            var authorIdParam = new SqlParameter("@AuthorId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(authorIdParam);
            await cmd.ExecuteNonQueryAsync();
            return Convert.ToInt32(authorIdParam.Value);
        }

        private async Task<int> InsertGenreAsync(SqlConnection connection, string genreName)
        {
            using var cmd = new SqlCommand("InsertGenre", connection) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@GenreName", genreName.Trim());
            var genreIdParam = new SqlParameter("@GenreId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(genreIdParam);
            await cmd.ExecuteNonQueryAsync();
            return Convert.ToInt32(genreIdParam.Value);
        }

        private async Task InsertBookAuthorAsync(SqlConnection connection, int bookId, int authorId)
        {
            using var cmd = new SqlCommand("InsertBookAuthor", connection) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@AuthorId", authorId);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task InsertBookGenreAsync(SqlConnection connection, int bookId, int genreId)
        {
            using var cmd = new SqlCommand("InsertBookGenre", connection) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@GenreId", genreId);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task InsertReviewAsync(SqlConnection connection, int bookId, int rating, string description)
        {
            using var cmd = new SqlCommand("InsertReview", connection) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@ReviewRating", rating);
            cmd.Parameters.AddWithValue("@ReviewDescription", description);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}