using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Dtos;

namespace LibraryManagementSystem.API.Services.Interfaces
{
    public interface ILibraryService
    {
        Task<IEnumerable<LibraryDto>> GetAllAsync();
        Task<ServiceResult<LibraryDto>> GetByIdAsync(int id);
        Task<ServiceResult<LibraryDto>> CreateAsync(CreateLibraryDto dto);
        Task<ServiceResult> UpdateAsync(int id, UpdateLibraryDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
