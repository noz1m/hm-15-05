using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Service;
using Infrastructure.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService)
{
    [HttpGet]
    public async Task<List<Book>> GetBooksAsync()
    {
        return await bookService.GetAllBookAsync();
    }
    [HttpGet("{id:int}")]
    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await bookService.GetBookByIdAsync(id);
    }
    [HttpPost]
    public async Task<string> CreateBookAsync(Book book)
    {
        return await bookService.CreateBookAsync(book);
    }
    [HttpPut]
    public async Task<string> UpdateBookAsync(Book book)
    {
        return await bookService.UpdateBookAsync(book);
    }
    [HttpDelete("{id:int}")]
    public async Task<string> DeleteBookAsync(int id)
    {
        return await bookService.DeleteBookAsync(id);
    }
    [HttpGet("GetMostBorrowedBook")]
    public async Task<Book?> GetMostBorrowedBookAsync()
    {
        return await bookService.GetMostBorrowedBookAsync();
    }
    [HttpGet(" GetBorrowedBooks")]
    public async Task<List<Book>> GetBorrowedBooksAsync()
    {
        return await bookService.GetBorrowedBooksAsync();
    }
    [HttpGet("GetUnavailableBooks")]
    public async Task<List<Book>> GetUnavailableBooksAsync()
    {
        return await bookService.GetUnavailableBooksAsync();
    }
    [HttpGet("GetMostPopularGenre")]
    public async Task<string> GetMostPopularGenreAsync()
    {
        return await bookService.GetMostPopularGenreAsync();
    }
}
