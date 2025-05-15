using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interface;

namespace Infrastructure.Service;

public class MemberService : IMemberService
{
    private readonly DataContext context = new DataContext();

    public async Task<List<Member>> GetAllMemberAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from members";
        var result = await connection.QueryAsync<Member>(sql);
        return result.ToList(); 
    }
    public async Task<Member?> GetMemberByIdAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from members where id = @id";
        var result = await connection.QueryFirstOrDefaultAsync<Member>(sql, new {id});
        return result == null ? null : result;
    }
    public async Task<string> CreateMemberAsync(Member member)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"insert into members (fullName,phone,email,membershipDate)
                    values (@fullName,@phone,@email,@membershipDate)";
        var result = await connection.ExecuteAsync(sql, member);
        return result > 0 ? "Member created successfully" : "Failed to create member";
    }
    public async Task<string> UpdateMemberAsync(Member member)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"update members set fullName=@fullName, phone=@phone, email=@email, membershipDate=@membershipDate
                    where id = @id";
        var result = await connection.ExecuteAsync(sql, member);
        return result > 0 ? "Member updated successfully" : "Failed to update member";
    }
    public async Task<string> DeleteMemberAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"delete from members where id=@id";
        var result = await connection.ExecuteAsync(sql, new {id});
        return result > 0 ? "Member deleted successfully" : "Failed to delete member";
    }
}
