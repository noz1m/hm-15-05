using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interface;

namespace Infrastructure.Service;

public class BorrowingService : IBorrowingService
{
    private readonly DataContext context = new DataContext();

    public async Task<List<Borrowing>> GetAllBorrowingAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from borrowings";
        var result = await connection.QueryAsync<Borrowing>(sql);
        return result.ToList();
    }
    public async Task<Borrowing?> GetBorrowingByIdAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from borrowing where id = @id";
        var result = await connection.QueryFirstOrDefaultAsync<Borrowing>(sql, new {id});
        return result == null ? null : result;
    }
    public async Task<string> CreateBorrowingAsync(Borrowing borrowing)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"insert into borrowings (bookId,memberId,borrowDate,returnDate,dueDate,fine)
                    values (@bookId,@memberId,@borrowDate,@returnDate,@dueDate,@fine)";
        var result = await connection.ExecuteAsync(sql, borrowing);
        return result > 0 ? "Borrowing created successfully" : "Failed to create borrowing";
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
        var result = await connection.ExecuteAsync(sql, new {id});
        return result > 0 ? "Borrowing deleted successfully" : "Failed to delete borrowing";
    }
}
