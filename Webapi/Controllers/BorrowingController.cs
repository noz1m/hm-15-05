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
public class BorrowingController(IBorrowingService borrowingService)
{
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
    [HttpGet("ReturnBook")]
    public async Task<string> ReturnBookAsync(int id)
    {
        return await borrowingService.ReturnBookAsync(id);
    }
    [HttpGet("GetTotalBorrowedBooks")]
    public async Task<int> GetTotalBorrowedBooksAsync()
    {
        return await borrowingService.GetTotalBorrowedBooksAsync();
    }
    [HttpGet("GetAverageFine")]
    public async Task<decimal> GetAverageFineAsync()
    {
        return await borrowingService.GetAverageFineAsync();
    }
    [HttpGet("GetNeverBorrowedBooksCount")]
    public async Task<int> GetNeverBorrowedBooksCountAsync()
    {
        return await borrowingService.GetNeverBorrowedBooksCountAsync();
    }
    [HttpGet("GetActiveBorrowersCount")]
    public async Task<int> GetActiveBorrowersCountAsync()
    {
        return await borrowingService.GetActiveBorrowersCountAsync();
    }
    [HttpGet("GetTotalFines")]
    public async Task<decimal> GetTotalFinesAsync()
    {
        return await borrowingService.GetTotalFinesAsync();
    }
    [HttpGet("GetCountOfLateReturns")]
    public async Task<int> GetCountOfLateReturnsAsync()
    {
        return await borrowingService.GetCountOfLateReturnsAsync();
    }
}
