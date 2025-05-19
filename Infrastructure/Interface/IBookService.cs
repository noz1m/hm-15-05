using System.Security.Cryptography.X509Certificates;
using Domain.Entities;

namespace Infrastructure.Interface;

public interface IBookService
{
    public Task<List<Book>> GetAllBookAsync();
    public Task<Book?> GetBookByIdAsync(int id);
    public Task<string> CreateBookAsync(Book book);
    public Task<string> UpdateBookAsync(Book book);
    public Task<string> DeleteBookAsync(int id);
    public Task<Book?> GetMostBorrowedBookAsync();
    public Task<List<Book>> GetBorrowedBooksAsync();
    public Task<List<Book>> GetUnavailableBooksAsync();
    public Task<string?> GetMostPopularGenreAsync();
}
