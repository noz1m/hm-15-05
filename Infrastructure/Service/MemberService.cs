using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interface;

namespace Infrastructure.Service;

public class MemberService(DataContext context) : IMemberService
{
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
        var result = await connection.QueryFirstOrDefaultAsync<Member>(sql, new { id });
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
        var result = await connection.ExecuteAsync(sql, new { id });
        return result > 0 ? "Member deleted successfully" : "Failed to delete member";
    }
    public async Task<Book?> GetMostBorrowedBookAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select b.* from books b
        join (
            select book_id, count(*) as borrow_count
            from borrowings
            group by book_id
            order by borrow_count desc
            limit 1
        ) as top_book on b.id = top_book.book_id;";
        var result = await connection.QuerySingleOrDefaultAsync<Book>(sql);
        return result;
    }
    public async Task<Member?> GetFirstMemberWithOverdueReturnsAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select m.* from members m
        join borrowings br on m.id = br.member_id
        where br.returnDate > br.dueDate
           or (br.returnDate is null and br.dueDate < now())
        order by br.dueDate asc
        limit 1;";
        var result = await connection.QueryFirstOrDefaultAsync<Member>(sql);
        return result;
    }
    public async Task<List<Member>> GetTop5BorrowersAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select m.*, count(br.id) as borrowCount from members m
        join borrowings br on m.id = br.memberId
        group by m.id
        order by borrowCount desc
        limit 5;";
        var result = await connection.QueryAsync<Member>(sql);
        return result.ToList();
    }
    public async Task<List<Member>> GetMembersWithFinesAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select distinct m.* from members m
        join borrowings br on m.id = br.memberId
        where br.fine > 0;";
        var result = await connection.QueryAsync<Member>(sql);
        return result.ToList();
    }
}
