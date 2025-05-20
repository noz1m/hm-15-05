using Domain.ApiResponse;
using Domain.Entities;

namespace Infrastructure.Interface;

public interface IBorrowingService
{
    public Task<Response<List<Borrowing>>> GetAllBorrowingAsync();
    public Task<Response<Borrowing>> GetBorrowingByIdAsync(int id);
    public Task<Response<string>> CreateBorrowingAsync(Borrowing borrowing);
    public Task<Response<string>> UpdateBorrowingAsync(Borrowing borrowing);
    public Task<Response<string>> DeleteBorrowingAsync(int id);
    public Task<Response<string>> ReturnBookAsync(int borrowingId);
    public Task<Response<int>> GetTotalBorrowedBooksAsync();
    public Task<Response<decimal>> GetAverageFineAsync();
    public Task<Response<int>> GetNeverBorrowedBooksCountAsync();
    public Task<Response<int>> GetActiveBorrowersCountAsync();
    public Task<Response<decimal>> GetTotalFinesAsync();
    public Task<Response<int>> GetCountOfLateReturnsAsync();
}
