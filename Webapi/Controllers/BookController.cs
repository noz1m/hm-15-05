using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Service;
using Infrastructure.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Domain.ApiResponse;

namespace Webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService)
{
    [HttpGet]
    public async Task<Response<List<Book>>> GetBooksAsync()
    {
        return await bookService.GetAllBookAsync();
    }
    [HttpGet("{id:int}")]
    public async Task<Response<Book>> GetBookByIdAsync(int id)
    {
        return await bookService.GetBookByIdAsync(id);
    }
    [HttpPost]
    public async Task<Response<string>> CreateBookAsync(Book book)
    {
        return await bookService.CreateBookAsync(book);
    }
    [HttpPut]
    public async Task<Response<string>> UpdateBookAsync(Book book)
    {
        return await bookService.UpdateBookAsync(book);
    }
    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteBookAsync(int id)
    {
        return await bookService.DeleteBookAsync(id);
    }
    [HttpGet("GetMostBorrowedBook")]
    public async Task<Response<Book>> GetMostBorrowedBookAsync()
    {
        return await bookService.GetMostBorrowedBookAsync();
    }
    [HttpGet(" GetBorrowedBooks")]
    public async Task<Response<List<Book>>> GetBorrowedBooksAsync()
    {
        return await bookService.GetBorrowedBooksAsync();
    }
    [HttpGet("GetUnavailableBooks")]
    public async Task<Response<List<Book>>> GetUnavailableBooksAsync()
    {
        return await bookService.GetUnavailableBooksAsync();
    }
    [HttpGet("GetMostPopularGenre")]
    public async Task<Response<string>> GetMostPopularGenreAsync()
    {
        return await bookService.GetMostPopularGenreAsync();
    }
}
