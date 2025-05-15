using Domain.Entities;

namespace Infrastructure.Interface;

public interface IMemberService
{
    public Task<List<Member>> GetAllMemberAsync();
    public Task<Member?> GetMemberByIdAsync(int id);
    public Task<string> CreateMemberAsync(Member member);
    public Task<string> UpdateMemberAsync(Member member);
    public Task<string> DeleteMemberAsync(int id);
}
