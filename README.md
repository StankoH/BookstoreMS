# BookstoreMS

ASP.NET Core Web API for managing books, authors, genres, and reviews.

## Setup

1. Create SQL Server database (`BookstoreDB`)
2. Update `appsettings.json` with your connection string:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BookstoreDB;Trusted_Connection=True;TrustServerCertificate=True"
}
