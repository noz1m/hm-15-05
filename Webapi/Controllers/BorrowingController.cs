using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Service;
using Infrastructure.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Interface;
using Domain.ApiResponse;

namespace Webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowingController(IBorrowingService borrowingService)
{
    [HttpGet]
    public async Task<Response<List<Borrowing>>> GetAllBorrowingAsync()
    {
        return await borrowingService.GetAllBorrowingAsync();
    }
    [HttpGet("{id:int}")]
    public async Task<Response<Borrowing>> GetBorrowingByIdAsync(int id)
    {
        return await borrowingService.GetBorrowingByIdAsync(id);
    }
    [HttpPost]
    public async Task<Response<string>> CreateBorrowingAsync(Borrowing borrowing)
    {
        return await borrowingService.CreateBorrowingAsync(borrowing);
    }
    [HttpPut]
    public async Task<Response<string>> UpdateBorrowingAsync(Borrowing borrowing)
    {
        return await borrowingService.UpdateBorrowingAsync(borrowing);
    }
    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteBorrowingAsync(int id)
    {
        return await borrowingService.DeleteBorrowingAsync(id);
    }
    [HttpGet("ReturnBook")]
    public async Task<Response<string>> ReturnBookAsync(int id)
    {
        return await borrowingService.ReturnBookAsync(id);
    }
    [HttpGet("GetTotalBorrowedBooks")]
    public async Task<Response<int>> GetTotalBorrowedBooksAsync()
    {
        return await borrowingService.GetTotalBorrowedBooksAsync();
    }
    [HttpGet("GetAverageFine")]
    public async Task<Response<decimal>> GetAverageFineAsync()
    {
        return await borrowingService.GetAverageFineAsync();
    }
    [HttpGet("GetNeverBorrowedBooksCount")]
    public async Task<Response<int>> GetNeverBorrowedBooksCountAsync()
    {
        return await borrowingService.GetNeverBorrowedBooksCountAsync();
    }
    [HttpGet("GetActiveBorrowersCount")]
    public async Task<Response<int>> GetActiveBorrowersCountAsync()
    {
        return await borrowingService.GetActiveBorrowersCountAsync();
    }
    [HttpGet("GetTotalFines")]
    public async Task<Response<decimal>> GetTotalFinesAsync()
    {
        return await borrowingService.GetTotalFinesAsync();
    }
    [HttpGet("GetCountOfLateReturns")]
    public async Task<Response<int>> GetCountOfLateReturnsAsync()
    {
        return await borrowingService.GetCountOfLateReturnsAsync();
    }
}
