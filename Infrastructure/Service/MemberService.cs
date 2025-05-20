using Dapper;
using Domain.ApiResponse;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interface;
using System.Net; 

namespace Infrastructure.Service;

public class MemberService(DataContext context) : IMemberService
{
    public async Task<Response<List<Member>>> GetAllMemberAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from members";
        var result = await connection.QueryAsync<Member>(sql);
        if (result == null)
        {
            return new Response<List<Member>>("Members not found", HttpStatusCode.NotFound);
        }
        return new Response<List<Member>>(result.ToList(), "Members found");
    }
    public async Task<Response<Member>> GetMemberByIdAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select * from members where id = @id";
        var result = await connection.QueryFirstOrDefaultAsync<Member>(sql, new { id });
        if (result == null)
        {
            return new Response<Member>("Member not found", HttpStatusCode.NotFound);
        }
        return new Response<Member>(result, "Member found");
    }
    public async Task<Response<string>> CreateMemberAsync(Member member)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"insert into members (fullName,phone,email,membershipDate)
                    values (@fullName,@phone,@email,@membershipDate)";
        var result = await connection.ExecuteAsync(sql, member);
        if (result > 0)
        {
            return new Response<string>("Member created successfully", HttpStatusCode.NotFound);
        }
        return new Response<string>(null, "Failed to create member");
    }
    public async Task<Response<string>> UpdateMemberAsync(Member member)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"update members set fullName=@fullName, phone=@phone, email=@email, membershipDate=@membershipDate
                    where id = @id";
        var result = await connection.ExecuteAsync(sql, member);
        if (result == 0)
        {
            return new Response<string>("Member not updated", HttpStatusCode.NotFound);
        }
        return new Response<string>(null, "Member updated");
    }
    public async Task<Response<string>> DeleteMemberAsync(int id)
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"delete from members where id=@id";
        var result = await connection.ExecuteAsync(sql, new { id });
        if (result == 0)
        {
            return new Response<string>("Member not deleted", HttpStatusCode.NotFound);
        }
        return new Response<string>(null, "Member deleted");
    }
    public async Task<Response<Book>> GetMostBorrowedBookAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select b.* from books b
        join (
            select book_id, count(*) as borrowCount
            from borrowings
            group by bookId
            order by borrow_count desc
            limit 1
        ) as top_book on b.id = topBook.bookId;";
        var result = await connection.QuerySingleOrDefaultAsync<Book>(sql);
        if (result == null)
        {
            return new Response<Book>("Could't Get most Borrowing book", HttpStatusCode.NotFound);
        }
        return new Response<Book>(result, "Not founded");
    }
    public async Task<Response<Member>> GetFirstMemberWithOverdueReturnsAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"select m.* from members m
        join borrowings br on m.id = br.memberId
        where br.returnDate > br.dueDate
           or (br.returnDate is null and br.dueDate < now())
        order by br.dueDate asc
        limit 1;";
        var result = await connection.QueryFirstOrDefaultAsync<Member>(sql);
        if (result == null)
        {
            return new Response<Member>("Could't Get first member with overdue returns", HttpStatusCode.NotFound);
        }
        return new Response<Member>(result, "Successfully founded");
    }
    public async Task<Response<List<Member>>> GetTop5BorrowersAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select m.*, count(br.id) as borrowCount from members m
        join borrowings br on m.id = br.memberId
        group by m.id
        order by borrowCount desc
        limit 5;";
        var result = await connection.QueryAsync<Member>(sql);
        if (result == null)
        {
            return new Response<List<Member>>("Could't Get top 5 borrowers", HttpStatusCode.NotFound);
        }
        return new Response<List<Member>>(result.ToList(), "Successfully founded");
    }
    public async Task<Response<List<Member>>> GetMembersWithFinesAsync()
    {
        using var connection = await context.GetNpgsqlConnection();
        var sql = @"
        select distinct m.* from members m
        join borrowings br on m.id = br.memberId
        where br.fine > 0;";
        var result = await connection.QueryAsync<Member>(sql);
        if (result == null)
        {
            return new Response<List<Member>>("Could't Get members with fines", HttpStatusCode.NotFound);
        }
        return new Response<List<Member>>(result.ToList(), "Successfully founded");
    }
}
