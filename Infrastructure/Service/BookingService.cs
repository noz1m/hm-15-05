using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Data;
using Dapper;

namespace Infrastructure.Service;

public class BookingService : IBookService
{
    private readonly DataContext context = new DataContext();

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
        var result = await connection.QueryFirstOrDefaultAsync<Book>(sql, new {id});
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
        var result = await connection.ExecuteAsync(sql, new {id});
        return result > 0 ? "Book deleted successfully" : "Failed to delete book";
    }
}
