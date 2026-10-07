using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Dtos;

namespace LibraryManagementSystem.API.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllAsync();
        Task<ServiceResult<MemberDto>> GetByIdAsync(int id);
        Task<ServiceResult<MemberDto>> CreateAsync(CreateMemberDto dto);
        Task<ServiceResult> UpdateAsync(int id, UpdateMemberDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
