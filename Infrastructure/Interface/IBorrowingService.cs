using Domain.Entities;

namespace Infrastructure.Interface;

public interface IBorrowingService
{
    public Task<List<Borrowing>> GetAllBorrowingAsync();
    public Task<Borrowing?> GetBorrowingByIdAsync(int id);
    public Task<string> CreateBorrowingAsync(Borrowing borrowing);
    public Task<string> UpdateBorrowingAsync(Borrowing borrowing);
    public Task<string> DeleteBorrowingAsync(int id);
}
