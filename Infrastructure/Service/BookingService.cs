using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Data;
using Dapper;

namespace Infrastructure.Service;

public class BookingService(DataContext context) : IBookService
{
    public async Task<List<Book>> GetAllBookAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from books";
        var result = await connection.QueryAsync<Book>(sql);
        return result.ToList();
    }
    public async Task<Book?> GetBookByIdAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from books where id = @id";
        var result = await connection.QueryFirstOrDefaultAsync<Book>(sql, new { id });
        return result == null ? null : result;
    }
    public async Task<string> CreateBookAsync(Book book)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"insert into books (title, genre, publicationYear, totalCopies, availableCopies) 
                    values (@title, @genre, @publicationYear, @totalCopies, @availableCopies)";
        var result = await connection.ExecuteAsync(sql, book);
        return result > 0 ? "Book created successfully" : "Failed to create book";
    }
    public async Task<string> UpdateBookAsync(Book book)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"update books set title=@title, genre=@genre, publicationYeaR=@publicationYear, totalCopies=@totalCopies, availableCopies=@availableCopies 
                    where id=@id";
        var result = await connection.ExecuteAsync(sql, book);
        return result > 0 ? "Book updated successfully" : "Failed to update book";
    }
    public async Task<string> DeleteBookAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"delete from books where id=@id";
        var result = await connection.ExecuteAsync(sql, new { id });
        return result > 0 ? "Book deleted successfully" : "Failed to delete book";
    }
    public async Task<Book?> GetMostBorrowedBookAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select b.* from books b
        join (
            select book_id, count(*) as borrow_count from borrowings
            group by book_id
            order by borrow_count desc
            limit 1
        ) as top_book on b.id = top_book.book_id;";
        var result = await connection.QuerySingleOrDefaultAsync<Book>(sql);
        return result == null ? null : result;
    }
    public async Task<List<Book>> GetBorrowedBooksAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select b.* from books b
        join borrowings br on b.id = br.book_id
        where br.return_date is null;";
        var books = await connection.QueryAsync<Book>(sql);
        return books.ToList();
    }
    public async Task<List<Book>> GetUnavailableBooksAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select b.* from books b
        where b.total_copies < (
            select count(*)
            from borrowings br
            where br.book_id = b.id and br.return_date is null);";
        var books = await connection.QueryAsync<Book>(sql);
        return books.ToList();
    }
    public async Task<string?> GetMostPopularGenreAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select b.genre from books b
        join borrowings br on b.id = br.bookId
        group by b.genre
        order by count(*) desc
        limit 1;";
        var result = await connection.QuerySingleOrDefaultAsync<string>(sql);
        return result;
    }
    public async Task<IEnumerable<Book>> GetBooksBorrowedMoreThan5TimesAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select b.* from books b
        join (
            select bookId
            from borrowings
            group by bookId
            having count(*) > 5
        ) as popularBooks on b.id = popularBooks.bookId;";
        var books = await connection.QueryAsync<Book>(sql);
        return books;
    }
}
