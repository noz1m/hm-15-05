using Infrastructure.Interface;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Dapper;
using Domain.Entities;

namespace Webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController(IMemberService memberService)
{
    [HttpGet]
    public async Task<List<Member>> GetMembersAsync()
    {
        return await memberService.GetAllMemberAsync();
    }
    [HttpGet("{id:int}")]
    public async Task<Member?> GetMemberByIdAsync(int id)
    {
        return await memberService.GetMemberByIdAsync(id);
    }
    [HttpPost]
    public async Task<string> CreateMemberAsync(Member member)
    {
        return await memberService.CreateMemberAsync(member);
    }
    [HttpPut]
    public async Task<string> UpdateMemberAsync(Member member)
    {
        return await memberService.UpdateMemberAsync(member);
    }
    [HttpDelete("{id:int}")]
    public async Task<string> DeleteMemberAsync(int id)
    {
        return await memberService.DeleteMemberAsync(id);
    }
    [HttpGet("GetMostBorrowedBook")]
    public async Task<Book?> GetMostBorrowedBookAsync()
    {
        return await memberService.GetMostBorrowedBookAsync();
    }
    [HttpGet("GetFirstMemberWithOverdueReturns")]
    public async Task<Member?> GetFirstMemberWithOverdueReturnsAsync()
    {
        return await memberService.GetFirstMemberWithOverdueReturnsAsync();
    }
    [HttpGet("GetTop5Borrowers")]
    public async Task<List<Member>> GetTop5BorrowersAsync()
    {
        return await memberService.GetTop5BorrowersAsync();
    }
    [HttpGet("GetMembersWithFines")]
    public async Task<List<Member>> GetMembersWithFinesAsync()
    {
        return await memberService.GetMembersWithFinesAsync();
    }
} 
