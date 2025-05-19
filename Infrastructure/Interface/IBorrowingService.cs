using Domain.Entities;

namespace Infrastructure.Interface;

public interface IBorrowingService
{
    public Task<List<Borrowing>> GetAllBorrowingAsync();
    public Task<Borrowing?> GetBorrowingByIdAsync(int id);
    public Task<string> CreateBorrowingAsync(Borrowing borrowing);
    public Task<string> UpdateBorrowingAsync(Borrowing borrowing);
    public Task<string> DeleteBorrowingAsync(int id);
    public Task<string> ReturnBookAsync(int borrowingId);
    public Task<int> GetTotalBorrowedBooksAsync();
    public Task<decimal> GetAverageFineAsync();
    public Task<int> GetNeverBorrowedBooksCountAsync();
    public Task<int> GetActiveBorrowersCountAsync();
    public Task<decimal> GetTotalFinesAsync();
    public Task<int> GetCountOfLateReturnsAsync();
}
