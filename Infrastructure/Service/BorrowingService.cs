using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interface;

namespace Infrastructure.Service;

public class BorrowingService(DataContext context) : IBorrowingService
{
    public async Task<List<Borrowing>> GetAllBorrowingAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from borrowings";
        var result = await connection.QueryAsync<Borrowing>(sql);
        return result.ToList();
    }
    public async Task<Borrowing?> GetBorrowingByIdAsync(int memberId)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from borrowings where memberId = @memberId";
        var result = await connection.QueryFirstOrDefaultAsync<Borrowing>(sql, new { memberId });
        return result == null ? null : result;
    }
    public async Task<string> CreateBorrowingAsync(Borrowing borrowing)
    {
        using var connection = await context.GetNpgsqlConnection();
        var bookCommand = @"select * from books where id = @id";
        var book = await connection.QueryFirstOrDefaultAsync<Book>(bookCommand, new { borrowing.BookId });
        if (book == null)
        {
            return "Book not found";
        }
        if (book.AvailableCopies <= 0)
        {
            return "Book is not available";
        }
        if (borrowing.BorrowDate >= borrowing.DueDate)
        {
            return "Borrow date must be before due date";
        }
        var sql = @"insert into borrowings (bookId,memberId,borrowDate,returnDate,dueDate,fine)
                    values (@bookId,@memberId,@borrowDate,@returnDate,@dueDate,@fine)";
        var result = await connection.ExecuteAsync(sql, borrowing);
        if (result == 0)
        {
            return "Failed to create borrowing";
        }
        var updateBookCommand = @"update books set avalaibleCopies = availableCopies - 1 where id = @id";
        await connection.ExecuteAsync(updateBookCommand, new { borrowing.BookId });
        return "Borrowing created successfully";
    }
    public async Task<string> ReturnBookAsync(int borrowingId)
    {
        using var connection = await context.GetNpgsqlConnection();
        var borrowingComand = @"select * from borrowings where id = @id";
        var borrowing = await connection.QueryFirstOrDefaultAsync<Borrowing>(borrowingComand, new { borrowingId });
        if (borrowing == null)
        {
            return "Borrowing not found";
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
            return "Failed to return book";
        }
        var updateBookCommand = @"update books set availableCopies = availableCopies + 1 where id = @id";
        await connection.ExecuteAsync(updateBookCommand, new { borrowing.BookId });
        return "Book returned successfully";
    }
    public async Task<string> UpdateBorrowingAsync(Borrowing borrowing)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"update borrowings set bookId=@bookId, memberId=@memberId, borrowDate=@borrowDate, returnDate=@returnDate, dueDate=@dueDate, fine=@fine 
                    where id=@id";
        var result = await connection.ExecuteAsync(sql, borrowing);
        return result > 0 ? "Borrowing updated successfully" : "Failed to update borrowing";
    }
    public async Task<string> DeleteBorrowingAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"delete from borrowings where id = @id";
        var result = await connection.ExecuteAsync(sql, new { id });
        return result > 0 ? "Borrowing deleted successfully" : "Failed to delete borrowing";
    }
    public async Task<int> GetTotalBorrowedBooksAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = "select count(*) from borrowings;";
        int result = await connection.ExecuteScalarAsync<int>(sql);
        return result;
    }
    public async Task<decimal> GetAverageFineAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = "select avg(fine) from borrowings where fine is not null;";
        var result = await connection.ExecuteScalarAsync<decimal?>(sql);
        if (result.HasValue)
            return result.Value;
        else return 0;
    }
    public async Task<int> GetNeverBorrowedBooksCountAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select count(*) from books b
        join borrowings br on b.id = br.bookId
        where br.id is null;";
        var result = await connection.ExecuteScalarAsync<int>(sql);
        return result;
    }
    public async Task<int> GetActiveBorrowersCountAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        const string sql = "select count(distinct memberId) from borrowings;";
        var result = await connection.ExecuteScalarAsync<int>(sql);
        return result;
    }
    public async Task<decimal> GetTotalFinesAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = "select sum(fine) from borrowings;";
        var result = await connection.QuerySingleOrDefaultAsync<decimal?>(sql);
        if (result.HasValue)
            return result.Value;
        else return 0;
    }
    public async Task<int> GetCountOfLateReturnsAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select count(*) from borrowings 
        where returnDate > dueDate; ";
        var result = await connection.ExecuteScalarAsync<int>(sql);
        return result;
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