using System.Security.Cryptography.X509Certificates;
using Domain.ApiResponse;
using Domain.Entities;

namespace Infrastructure.Interface;

public interface IBookService
{
    public Task<Response<List<Book>>> GetAllBookAsync();
    public Task<Response<Book>> GetBookByIdAsync(int id);
    public Task<Response<string>> CreateBookAsync(Book book);
    public Task<Response<string>> UpdateBookAsync(Book book);
    public Task<Response<string>> DeleteBookAsync(int id);
    public Task<Response<Book>> GetMostBorrowedBookAsync();
    public Task<Response<List<Book>>> GetBorrowedBooksAsync();
    public Task<Response<List<Book>>> GetUnavailableBooksAsync();
    public Task<Response<string>> GetMostPopularGenreAsync();
}
