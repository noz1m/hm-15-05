using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Service;
using Infrastructure.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController
{
    private readonly IBookService bookService = new BookingService();

    [HttpGet]
    public async Task<List<Book>> GetBooksAsync()
    {
        return await bookService.GetAllBookAsync();
    }

    [HttpGet("{id}")]
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

    [HttpDelete("{id}")]
    public async Task<string> DeleteBookAsync(int id)
    {
        return await bookService.DeleteBookAsync(id);
    }
}
