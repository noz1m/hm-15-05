using Domain.ApiResponse;
using Domain.Entities;

namespace Infrastructure.Interface;

public interface IMemberService
{
    public Task<Response<List<Member>>> GetAllMemberAsync();
    public Task<Response<Member>> GetMemberByIdAsync(int id);
    public Task<Response<string>> CreateMemberAsync(Member member);
    public Task<Response<string>> UpdateMemberAsync(Member member);
    public Task<Response<string>> DeleteMemberAsync(int id);
    public Task<Response<Book>> GetMostBorrowedBookAsync();
    public Task<Response<Member>> GetFirstMemberWithOverdueReturnsAsync();
    public Task<Response<List<Member>>> GetTop5BorrowersAsync();
    public Task<Response<List<Member>>> GetMembersWithFinesAsync();
}
