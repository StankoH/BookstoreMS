USE [master]
GO
/****** Object:  Database [Digacon]    Script Date: 17.7.2025. 11:49:49 ******/
CREATE DATABASE [Digacon]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Digacon', FILENAME = N'D:\Development\MSSQL\MSSQL12.MSSQLSERVER\MSSQL\DATA\Digacon.mdf' , SIZE = 69632KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Digacon_log', FILENAME = N'D:\Development\MSSQL\MSSQL12.MSSQLSERVER\MSSQL\DATA\Digacon_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Digacon] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Digacon].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Digacon] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Digacon] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Digacon] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Digacon] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Digacon] SET ARITHABORT OFF 
GO
ALTER DATABASE [Digacon] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Digacon] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Digacon] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Digacon] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Digacon] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Digacon] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Digacon] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Digacon] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Digacon] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Digacon] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Digacon] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Digacon] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Digacon] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Digacon] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Digacon] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Digacon] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Digacon] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Digacon] SET RECOVERY FULL 
GO
ALTER DATABASE [Digacon] SET  MULTI_USER 
GO
ALTER DATABASE [Digacon] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Digacon] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Digacon] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Digacon] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Digacon] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Digacon', N'ON'
GO
USE [Digacon]
GO
/****** Object:  Table [dbo].[AppRole]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRole](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Author]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Author](
	[AuthorId] [int] IDENTITY(1,1) NOT NULL,
	[AuthorName] [varchar](max) NOT NULL,
	[AuthorYearOfBirth] [int] NOT NULL,
 CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED 
(
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book](
	[BookId] [int] IDENTITY(1,1) NOT NULL,
	[BookTitle] [varchar](max) NOT NULL,
	[BookPrice] [money] NOT NULL,
	[BookDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookAuthor]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookAuthor](
	[BookId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookGenre]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookGenre](
	[BookId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_BookGenre] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC,
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genre]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genre](
	[GenreId] [int] IDENTITY(1,1) NOT NULL,
	[GenreName] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED 
(
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Review]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[ReviewId] [int] IDENTITY(1,1) NOT NULL,
	[ReviewRating] [int] NOT NULL,
	[ReviewDescription] [nvarchar](max) NOT NULL,
	[BookId] [int] NOT NULL,
 CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AppRole] ([RoleId])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_Role]
GO
ALTER TABLE [dbo].[BookAuthor]  WITH CHECK ADD  CONSTRAINT [FK_BookAuthor_Author1] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Author] ([AuthorId])
GO
ALTER TABLE [dbo].[BookAuthor] CHECK CONSTRAINT [FK_BookAuthor_Author1]
GO
ALTER TABLE [dbo].[BookAuthor]  WITH CHECK ADD  CONSTRAINT [FK_BookAuthor_Book] FOREIGN KEY([BookId])
REFERENCES [dbo].[Book] ([BookId])
GO
ALTER TABLE [dbo].[BookAuthor] CHECK CONSTRAINT [FK_BookAuthor_Book]
GO
ALTER TABLE [dbo].[BookGenre]  WITH CHECK ADD  CONSTRAINT [FK_BookGenre_Book] FOREIGN KEY([BookId])
REFERENCES [dbo].[Book] ([BookId])
GO
ALTER TABLE [dbo].[BookGenre] CHECK CONSTRAINT [FK_BookGenre_Book]
GO
ALTER TABLE [dbo].[BookGenre]  WITH CHECK ADD  CONSTRAINT [FK_BookGenre_Genre] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genre] ([GenreId])
GO
ALTER TABLE [dbo].[BookGenre] CHECK CONSTRAINT [FK_BookGenre_Genre]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_Book1] FOREIGN KEY([BookId])
REFERENCES [dbo].[Book] ([BookId])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_Book1]
GO
/****** Object:  StoredProcedure [dbo].[DeleteBook]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteBook]
    @BookId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Review WHERE BookId = @BookId;
    DELETE FROM BookAuthor WHERE BookId = @BookId;
    DELETE FROM BookGenre WHERE BookId = @BookId;
    DELETE FROM Book WHERE BookId = @BookId;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllBooks]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllBooks]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.BookId,
        b.BookTitle,
        b.BookPrice,
        ISNULL(AVG(r.ReviewRating), 0) AS AverageRating
    FROM Book b
    LEFT JOIN Review r ON b.BookId = r.BookId
    GROUP BY b.BookId, b.BookTitle, b.BookPrice
    ORDER BY b.BookTitle;
END
GO
/****** Object:  StoredProcedure [dbo].[GetTop10BooksByAvgRating]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTop10BooksByAvgRating]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 10
        b.BookId,
        b.BookTitle,
        b.BookPrice,
        AVG(r.ReviewRating * 1.0) AS AverageRating
    FROM Book b
    JOIN Review r ON b.BookId = r.BookId
    GROUP BY b.BookId, b.BookTitle, b.BookPrice
    ORDER BY AVG(r.ReviewRating) DESC, COUNT(*) DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[InsertAuthor]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertAuthor]
    @AuthorName VARCHAR(MAX),
    @AuthorId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @AuthorId = AuthorId
    FROM Author
    WHERE AuthorName = @AuthorName;

    IF @AuthorId IS NULL
    BEGIN
        INSERT INTO Author (AuthorName, AuthorYearOfBirth)
        VALUES (@AuthorName, 1990);

        SET @AuthorId = SCOPE_IDENTITY();
    END
END

GO
/****** Object:  StoredProcedure [dbo].[InsertAuthorByNameOnly]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertAuthorByNameOnly]
    @AuthorName VARCHAR(MAX),
    @AuthorId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NormalizedName VARCHAR(MAX) = LTRIM(RTRIM(LOWER(@AuthorName)));

    SELECT @AuthorId = AuthorId
    FROM Author
    WHERE LOWER(LTRIM(RTRIM(AuthorName))) = @NormalizedName;

    IF @AuthorId IS NULL
    BEGIN
        -- Ako ne postoji, dodaj autora s default vrijednošću
        INSERT INTO Author (AuthorName, AuthorYearOfBirth)
        VALUES (@AuthorName, 1970);

        SET @AuthorId = SCOPE_IDENTITY();
    END
END
GO
/****** Object:  StoredProcedure [dbo].[InsertBook]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertBook]
    @BookTitle VARCHAR(MAX),
    @BookPrice MONEY,
    @BookId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Book (BookTitle, BookPrice)
    VALUES (@BookTitle, @BookPrice);

    SET @BookId = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[InsertBookAuthor]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertBookAuthor]
    @BookId INT,
    @AuthorId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM BookAuthor
        WHERE BookId = @BookId AND AuthorId = @AuthorId
    )
    BEGIN
        INSERT INTO BookAuthor (BookId, AuthorId)
        VALUES (@BookId, @AuthorId);
    END
END
GO
/****** Object:  StoredProcedure [dbo].[InsertBookGenre]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertBookGenre]
    @BookId INT,
    @GenreId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM BookGenre
        WHERE BookId = @BookId AND GenreId = @GenreId
    )
    BEGIN
        INSERT INTO BookGenre (BookId, GenreId)
        VALUES (@BookId, @GenreId);
    END
END
GO
/****** Object:  StoredProcedure [dbo].[InsertGenre]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertGenre]
    @GenreName VARCHAR(MAX),
    @GenreId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NormalizedName VARCHAR(MAX) = LTRIM(RTRIM(LOWER(@GenreName)));

    SELECT @GenreId = GenreId
    FROM Genre
    WHERE LOWER(LTRIM(RTRIM(GenreName))) = @NormalizedName;

    IF @GenreId IS NULL
    BEGIN
        INSERT INTO Genre (GenreName)
        VALUES (@GenreName);

        SET @GenreId = SCOPE_IDENTITY();
    END
END
GO
/****** Object:  StoredProcedure [dbo].[InsertReview]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertReview]
    @BookId INT,
    @ReviewRating INT,
    @ReviewDescription NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Review (BookId, ReviewRating, ReviewDescription)
    VALUES (@BookId, @ReviewRating, @ReviewDescription);
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateBookPrice]    Script Date: 17.7.2025. 11:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateBookPrice]
    @BookId INT,
    @NewPrice MONEY
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Book
    SET BookPrice = @NewPrice
    WHERE BookId = @BookId;
END
GO
USE [master]
GO
ALTER DATABASE [Digacon] SET  READ_WRITE 
GO
