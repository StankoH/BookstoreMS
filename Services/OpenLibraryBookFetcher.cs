using BookstoreMS.Models.DTOs;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookstoreMS.Services
{
    public class OpenLibraryBookFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly List<string> _knownGenres = new()
        {
            "Action", "Adult", "Adventure", "Biography", "Comedy", "Crime", "Documentary", "Drama",
            "Family", "Fantasy", "History", "Horror", "Mystery", "News", "Romance", "Sci-Fi",
            "Self-Help", "Philosophy", "Thriller", "War", "Western", "Sport"
        };

        public OpenLibraryBookFetcher()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<BookImportDto>> FetchBooksAsync(string searchTerm, int limit, int offset)
        {
            var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(searchTerm)}&limit={limit}&offset={offset}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var data = JsonSerializer.Deserialize<OpenLibrarySearchResult>(response);
                var random = new Random();

                var books = new List<BookImportDto>();

                if (data?.Docs != null)
                {
                    foreach (var doc in data.Docs.Where(d => !string.IsNullOrWhiteSpace(d.Title)))
                    {
                        string description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
                        List<string> genres = new();

                        if (!string.IsNullOrWhiteSpace(doc.Key))
                        {
                            var workDetailUrl = $"https://openlibrary.org{doc.Key}.json";

                            try
                            {
                                var workResponse = await _httpClient.GetStringAsync(workDetailUrl);
                                var workData = JsonSerializer.Deserialize<OpenLibraryWorkDetail>(workResponse);

                                if (workData?.Description is JsonElement element)
                                {
                                    if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("value", out var valueProp))
                                        description = valueProp.GetString() ?? description;
                                    else if (element.ValueKind == JsonValueKind.String)
                                        description = element.GetString() ?? description;
                                }

                                if (workData?.Subjects != null && workData.Subjects.Count > 0)
                                {
                                    genres = workData.Subjects
                                        .Where(s => _knownGenres.Contains(s, StringComparer.OrdinalIgnoreCase))
                                        .Distinct()
                                        .Take(3)
                                        .ToList();
                                }
                            }
                            catch
                            {
                            }
                        }

                        if (genres.Count == 0)
                        {
                            genres = _knownGenres.OrderBy(_ => random.Next()).Take(random.Next(1, 4)).ToList();
                        }

                        books.Add(new BookImportDto
                        {
                            Title = doc.Title,
                            Authors = doc.AuthorName ?? new List<string>(),
                            Description = description,
                            Genres = genres,
                            Price = Math.Round((decimal)(random.NextDouble() * 29 + 1), 2)
                        });
                    }
                }

                return books;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error offset {offset}: {ex.Message}");
                return new List<BookImportDto>();
            }
        }

        public async Task SaveBooksToJsonAsync(string path)
        {
            const int totalBooksTarget = 400_000;
            const int batchSize = 50;
            const string searchTerm = "book";

            var allBooks = new List<BookImportDto>();
            int offset = 0;

            while (allBooks.Count < totalBooksTarget)
            {
                var books = await FetchBooksAsync(searchTerm, batchSize, offset);

                if (books.Count == 0)
                {
                }
                else
                {
                    allBooks.AddRange(books);
                    Console.WriteLine($"Fetched {allBooks.Count} books...");
                }

                offset += batchSize;
                await Task.Delay(300);
            }

            var json = JsonSerializer.Serialize(allBooks.Take(totalBooksTarget), new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            await File.WriteAllTextAsync(path, json);

            Console.WriteLine($"Saved {totalBooksTarget} books to {path}");
        }

        // Internal types
        private class OpenLibrarySearchResult
        {
            [JsonPropertyName("docs")]
            public List<OpenLibraryBookDoc> Docs { get; set; }
        }

        private class OpenLibraryBookDoc
        {
            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("author_name")]
            public List<string> AuthorName { get; set; }

            [JsonPropertyName("key")]
            public string Key { get; set; }
        }

        private class OpenLibraryWorkDetail
        {
            [JsonPropertyName("description")]
            public JsonElement? Description { get; set; }

            [JsonPropertyName("subjects")]
            public List<string> Subjects { get; set; }
        }
    }
}