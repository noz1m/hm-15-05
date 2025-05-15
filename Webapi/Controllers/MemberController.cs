using Infrastructure.Interface;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Dapper;
using Domain.Entities;

namespace Webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController
{
    private readonly IMemberService memberService = new MemberService();

    [HttpGet]
    public async Task<List<Member>> GetMembersAsync()
    {
        return await memberService.GetAllMemberAsync();
    }
    [HttpGet("{id}")]
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
    [HttpDelete]
    public async Task<string> DeleteMemberAsync(int id)
    {
        return await memberService.DeleteMemberAsync(id);
    }
} 
