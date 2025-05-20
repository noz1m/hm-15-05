using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Data;
using Dapper;
using Domain.ApiResponse;
using System.Net;

namespace Infrastructure.Service;

public class BookingService(DataContext context) : IBookService
{
    public async Task<Response<List<Book>>> GetAllBookAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from books";
        var result = await connection.QueryAsync<Book>(sql);
        if (result == null)
        {
            return new Response<List<Book>>("Book not found", HttpStatusCode.NotFound);
        }
        return new Response<List<Book>>(result.ToList(), "Book found");
    }
    public async Task<Response<Book>> GetBookByIdAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from books where id = @id";
        var result = await connection.QueryFirstOrDefaultAsync<Book>(sql, new { id });
        if (result == null)
        {
            return new Response<Book>("Book not found", HttpStatusCode.NotFound);
        }
        return new Response<Book>(result, "Book found");
    }
    public async Task<Response<string>> CreateBookAsync(Book book)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"insert into books (title, genre, publicationYear, totalCopies, availableCopies) 
                    values (@title, @genre, @publicationYear, @totalCopies, @availableCopies)";
        var result = await connection.ExecuteAsync(sql, book);
        if (result == 0)
        {
            return new Response<string>("Book not created", HttpStatusCode.NotFound);
        }
        return new Response<string>(null, "Book created successfully");
    }
    public async Task<Response<string>> UpdateBookAsync(Book book)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"update books set title=@title, genre=@genre, publicationYeaR=@publicationYear, totalCopies=@totalCopies, availableCopies=@availableCopies 
                    where id=@id";
        var result = await connection.ExecuteAsync(sql, book);
        if (result == 0)
        {
            return new Response<string>("Book not updated", HttpStatusCode.NotFound);
        }
        return new Response<string>(null, "Book updated");
    }
    public async Task<Response<string>> DeleteBookAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"delete from books where id=@id";
        var result = await connection.ExecuteAsync(sql, new { id });
        if (result == 0)
        {
            return new Response<string>("Book not deleted", HttpStatusCode.NotFound);
        }
        return new Response<string>(null, "Book deleted");
    }
    public async Task<Response<Book>> GetMostBorrowedBookAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select b.* from books b
        join (
            select bookId, count(*) as borrowCount from borrowings
            group by bookId
            order by borrowCount desc
            limit 1
        ) as top_book on b.id = top_book.bookId;";
        var result = await connection.QuerySingleOrDefaultAsync<Book>(sql);
        if (result == null)
        {
            return new Response<Book>("Could't Get most Borrowing book", HttpStatusCode.NotFound);
        }
        return new Response<Book>(result, "Not founded");
    }
    public async Task<Response<List<Book>>> GetBorrowedBooksAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select b.* from books b
        join borrowings br on b.id = br.bookId
        where br.returnDate is null;";
        var result = await connection.QueryAsync<Book>(sql);
        if (result == null)
        {
            return new Response<List<Book>>("Could't Get Borrowed books", HttpStatusCode.NotFound);
        }
        return new Response<List<Book>>(result.ToList(), "Successfully founded");
    }
    public async Task<Response<List<Book>>> GetUnavailableBooksAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select b.* from books b
        where b.totalCopies < (
            select count(*)
            from borrowings br
            where br.bookId = b.id and br.returnDate is null);";
        var result = await connection.QueryAsync<Book>(sql);
        if (result == null)
        {
            return new Response<List<Book>>("Gould't Get Unavailable books", HttpStatusCode.NotFound);
        }
        return new Response<List<Book>>(result.ToList(), "Successfully founded");
    }
    public async Task<Response<string>> GetMostPopularGenreAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select b.genre from books b
        join borrowings br on b.id = br.bookId
        group by b.genre
        order by count(*) desc
        limit 1;";
        var result = await connection.QuerySingleOrDefaultAsync<string>(sql);
        if (result == null)
        {
            return new Response<string>("Could't Get most popular genre", HttpStatusCode.NotFound);
        }
        return new Response<string>(result,"Successfully founded");
    }
    public async Task<Response<List<Book>>> GetBooksBorrowedMoreThan5TimesAsync()
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
        var result = await connection.QueryAsync<Book>(sql);
        if (result == null)
        {
            return new Response<List<Book>>("Could't Get book borrowed more than 5 times", HttpStatusCode.NotFound);
        }
        return new Response<List<Book>>(result.ToList(), "Successfully founded");
    }
}
