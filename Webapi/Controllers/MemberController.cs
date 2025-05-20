using Infrastructure.Interface;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Dapper;
using Domain.Entities;
using Domain.ApiResponse;

namespace Webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController(IMemberService memberService)
{
    [HttpGet]
    public async Task<Response<List<Member>>> GetMembersAsync()
    {
        return await memberService.GetAllMemberAsync();
    }
    [HttpGet("{id:int}")]
    public async Task<Response<Member>> GetMemberByIdAsync(int id)
    {
        return await memberService.GetMemberByIdAsync(id);
    }
    [HttpPost]
    public async Task<Response<string>> CreateMemberAsync(Member member)
    {
        return await memberService.CreateMemberAsync(member);
    }
    [HttpPut]
    public async Task<Response<string>> UpdateMemberAsync(Member member)
    {
        return await memberService.UpdateMemberAsync(member);
    }
    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteMemberAsync(int id)
    {
        return await memberService.DeleteMemberAsync(id);
    }
    [HttpGet("GetMostBorrowedBook")]
    public async Task<Response<Book>> GetMostBorrowedBookAsync()
    {
        return await memberService.GetMostBorrowedBookAsync();
    }
    [HttpGet("GetFirstMemberWithOverdueReturns")]
    public async Task<Response<Member>> GetFirstMemberWithOverdueReturnsAsync()
    {
        return await memberService.GetFirstMemberWithOverdueReturnsAsync();
    }
    [HttpGet("GetTop5Borrowers")]
    public async Task<Response<List<Member>>> GetTop5BorrowersAsync()
    {
        return await memberService.GetTop5BorrowersAsync();
    }
    [HttpGet("GetMembersWithFines")]
    public async Task<Response<List<Member>>> GetMembersWithFinesAsync()
    {
        return await memberService.GetMembersWithFinesAsync();
    }
} 
