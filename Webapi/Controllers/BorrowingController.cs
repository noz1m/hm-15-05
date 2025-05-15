using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Service;
using Infrastructure.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Interface;

namespace Webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowingController
{
    private readonly IBorrowingService borrowingService = new BorrowingService();

    [HttpGet]
    public async Task<List<Borrowing>> GetAllBorrowingAsync()
    {
        return await borrowingService.GetAllBorrowingAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<Borrowing?> GetBorrowingByIdAsync(int id)
    {
        return await borrowingService.GetBorrowingByIdAsync(id);
    }

    [HttpPost]
    public async Task<string> CreateBorrowingAsync(Borrowing borrowing)
    {
        return await borrowingService.CreateBorrowingAsync(borrowing);
    }

    [HttpPut]
    public async Task<string> UpdateBorrowingAsync(Borrowing borrowing)
    {
        return await borrowingService.UpdateBorrowingAsync(borrowing);
    }

    [HttpDelete("{id:int}")]
    public async Task<string> DeleteBorrowingAsync(int id)
    {
        return await borrowingService.DeleteBorrowingAsync(id);
    }
}
