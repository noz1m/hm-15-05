using Dapper;
using Domain.ApiResponse;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interface;
using System.Net;

namespace Infrastructure.Service;

public class BorrowingService(DataContext context) : IBorrowingService
{
    public async Task<Response<List<Borrowing>>> GetAllBorrowingAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from borrowings";
        var result = await connection.QueryAsync<Borrowing>(sql);
        if (result == null)
        {
            return new Response<List<Borrowing>>("Borrowings not found", HttpStatusCode.NotFound);
        }
        return new Response<List<Borrowing>>(result.ToList(), "Borrowings found");
    }
    public async Task<Response<Borrowing>> GetBorrowingByIdAsync(int memberId)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from borrowings where memberId = @memberId";
        var result = await connection.QueryFirstOrDefaultAsync<Borrowing>(sql, new { memberId });
        if (result == null)
        {
            return new Response<Borrowing>("Borrowing not founded", HttpStatusCode.NotFound);
        }
        return new Response<Borrowing>(result,"Borrowing founded");
    }
    public async Task<Response<string>> CreateBorrowingAsync(Borrowing borrowing)
    {
        using var connection = await context.GetNpgsqlConnection();
        var bookCommand = @"select * from books where id = @id";
        var book = await connection.QueryFirstOrDefaultAsync<Book>(bookCommand, new { borrowing.BookId });
        if (book == null)
        {
            return new Response<string>("Book not found",HttpStatusCode.NotFound);
        }
        if (book.AvailableCopies <= 0)
        {
            return new Response<string>("Book is not available", HttpStatusCode.NotFound);
        }
        if (borrowing.BorrowDate >= borrowing.DueDate)
        {
            return new Response<string>("Borrow date must be before due date", HttpStatusCode.BadRequest);
        }
        var sql = @"insert into borrowings (bookId,memberId,borrowDate,returnDate,dueDate,fine)
                    values (@bookId,@memberId,@borrowDate,@returnDate,@dueDate,@fine)";
        var result = await connection.ExecuteAsync(sql, borrowing);
        if (result == 0)
        {
            return new Response<string>("Failed to create borrowing", HttpStatusCode.BadRequest);
        }
        var updateBookCommand = @"update books set avalaibleCopies = availableCopies - 1 where id = @id";
        await connection.ExecuteAsync(updateBookCommand, new { borrowing.BookId });
        return new Response<string>("Successfully created borrowing", HttpStatusCode.Created);
    }
    public async Task<Response<string>> ReturnBookAsync(int borrowingId)
    {
        using var connection = await context.GetNpgsqlConnection();
        var borrowingComand = @"select * from borrowings where id = @id";
        var borrowing = await connection.QueryFirstOrDefaultAsync<Borrowing>(borrowingComand, new { borrowingId });
        if (borrowing == null)
        {
            return new Response<string>("Borrowing not found", HttpStatusCode.NotFound);
        }
        borrowing.ReturnDate = DateTime.Now;
        if (borrowing.ReturnDate > borrowing.DueDate)
        {
            var days = borrowing.ReturnDate.Day - borrowing.DueDate.Day;
            borrowing.Fine = days * 5;
        }
        var updateBorrowingCommanf = @"update borrowings set returnDate = @returnDate, fine = @fine where id = @id";
        var result = await connection.ExecuteAsync(updateBorrowingCommanf, borrowing);
        if (result == 0)
        {
            return new Response<string>("Failed to return book", HttpStatusCode.BadRequest);
        }
        var updateBookCommand = @"update books set availableCopies = availableCopies + 1 where id = @id";
        await connection.ExecuteAsync(updateBookCommand, new { borrowing.BookId });
        return new Response<string>(null,"Successfully returned book");
    }
    public async Task<Response<string>> UpdateBorrowingAsync(Borrowing borrowing)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"update borrowings set bookId=@bookId, memberId=@memberId, borrowDate=@borrowDate, returnDate=@returnDate, dueDate=@dueDate, fine=@fine 
                    where id=@id";
        var result = await connection.ExecuteAsync(sql, borrowing);
        if (result == 0)
        {
            return new Response<string>("Failed to update borrowing", HttpStatusCode.BadRequest);
        }
        return new Response<string>(null, "Successfully updated borrowing");
    }
    public async Task<Response<string>> DeleteBorrowingAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"delete from borrowings where id = @id";
        var result = await connection.ExecuteAsync(sql, new { id });
        if (result == 0)
        {
            return new Response<string>("Failed to delete borrowing", HttpStatusCode.BadRequest);
        }
        return new Response<string>(null,"Successfully deleted borrowing");
    }
    public async Task<Response<int>> GetTotalBorrowedBooksAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = "select count(*) from borrowings;";
        int result = await connection.ExecuteScalarAsync<int>(sql);
        if (result == 0)
        {
            return new Response<int>("Borrowings not found", HttpStatusCode.NotFound);
        }
        return new Response<int>(result, "Borrowings found");
    }
    public async Task<Response<decimal>> GetAverageFineAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = "select avg(fine) from borrowings where fine is not null;";
        var result = await connection.ExecuteScalarAsync<decimal?>(sql);
        if (result == null)
        {
            return new Response<decimal>("Average fine not found", HttpStatusCode.NotFound);
        }
        return new Response<decimal>(decimal.Round(result.Value, 2), "Average fine found");
    }
    public async Task<Response<int>> GetNeverBorrowedBooksCountAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select count(*) from books b
        join borrowings br on b.id = br.bookId
        where br.id is null;";
        var result = await connection.ExecuteScalarAsync<int>(sql);
        if (result == 0)
        {
            return new Response<int>("Never borrowed books not found", HttpStatusCode.NotFound);
        }
        return new Response<int>(result, "Never borrowed books found");
    }
    public async Task<Response<int>> GetActiveBorrowersCountAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        const string sql = "select count(distinct memberId) from borrowings;";
        var result = await connection.ExecuteScalarAsync<int>(sql);
        if (result == 0)
        {
            return new Response<int>("Active borrowers not found", HttpStatusCode.NotFound);
        }
        return new Response<int>(result, "Active borrowers found");
    }
    public async Task<Response<decimal>> GetTotalFinesAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = "select sum(fine) from borrowings;";
        var result = await connection.QuerySingleOrDefaultAsync<decimal?>(sql);
        if (result == null)
        {
            return new Response<decimal>("Total fines not found", HttpStatusCode.NotFound);
        }
        return new Response<decimal>(decimal.Round(result.Value, 2), "Total fines found");
    }
    public async Task<Response<int>> GetCountOfLateReturnsAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select count(*) from borrowings 
        where returnDate > dueDate; ";
        var result = await connection.ExecuteScalarAsync<int>(sql);
        if (result == 0)
        {
            return new Response<int>("Late returns not found", HttpStatusCode.NotFound);
        }
        return new Response<int>(result,"Late returns found");
    }
}

// public async Task<List<Borrowing>> GetAllBorrowingAsync()
// {
//     using var connection = await context.GetNpgsqlConnection();
//     var sql = @"select * from borrowings";
//     var result = await connection.QueryAsync<Borrowing>(sql);
//     return result.ToList();
// }
// public async Task<Borrowing?> GetBorrowingByIdAsync(int id)
// {
//     using var connection = await context.GetNpgsqlConnection();
//     var sql = @"select * from borrowing where id = @id";
//     var result = await connection.QueryFirstOrDefaultAsync<Borrowing>(sql, new {id});
//     return result == null ? null : result;
// }
// public async Task<string> CreateBorrowingAsync(Borrowing borrowing)
// {
//     using var connection = await context.GetNpgsqlConnection();
//     var sql = @"insert into borrowings (bookId,memberId,borrowDate,returnDate,dueDate,fine)
//                 values (@bookId,@memberId,@borrowDate,@returnDate,@dueDate,@fine)";
//     var result = await connection.ExecuteAsync(sql, borrowing);
//     return result > 0 ? "Borrowing created successfully" : "Failed to create borrowing";
// }
// public async Task<string> BorrowingBookAsnync(Borrowing borrowing)
// {
//     using var connection = await context.GetNpgsqlConnection();
//     var availableCopies = await connection.QueryFirstOrDefaultAsync<int>
//     (
//         @"select availableCopies from books where id = @id",
//         new {Id = borrowing.Id}
//     );
//     if (availableCopies <= 0)
//     {
//         return "Book is not available";
//     }
//     var sql = @"insert into borrowings (bookId,memberId,borrowDate,returnDate,dueDate,fine)
//                 values (@bookId,@memberId,@borrowDate,@returnDate,@dueDate,@fine)";
//     var sqlResult = "update books set availableCopies = availableCopies - 1 where id = @id";
//     var result = await connection.ExecuteAsync(sql, borrowing);
//     await connection.ExecuteAsync(sqlResult, new {Id = borrowing.Id});
//     return result > 0 ? "Book borrowed successfully" : "Failed to borrow book";
// }
// public async Task<string> ReturnBookAsync(int id, DateTime returnDate)
// {
//     using var connection = await context.GetNpgsqlConnection();
//     var borrowing = await connection.QueryFirstOrDefaultAsync<Borrowing>
//     (
//         @"select * from borrowings where id = @id", new {id = id} );
//     if (borrowing == null)
//     {
//         return "Borrowing not found";
//     }
//     if (borrowing.ReturnDate != null)
//     {
//         return "Book is already returned";
//     }
//     decimal fine = 0;
//     if (returnDate > borrowing.DueDate)
//     {
//         var daysLate = (returnDate - borrowing.DueDate).Days;
//         fine = daysLate * 1;
//     }
//     var updateBorrowing = @"update borrowings set returnDate = @returnDate, fine = @fine where id = @id";

//     var result = await connection.ExecuteAsync(updateBorrowing, new {returnDate, fine, id});

//     var updateBook = @"update books set availableCopies = availableCopies + 1 where id = @id";
//     await connection.ExecuteAsync(updateBook, new {Id = borrowing.Id}); 
//     return result > 0 ? "Book returned successfully" : "Failed to return book";
// }
// public async Task<string> BorrowBookAsync(Borrowing borrowing)
// {
//     using var connection = await context.GetNpgsqlConnection();
//     var 
// }

