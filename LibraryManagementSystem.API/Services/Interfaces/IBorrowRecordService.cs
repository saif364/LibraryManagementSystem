using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Dtos;

namespace LibraryManagementSystem.API.Services.Interfaces
{
    public interface IBorrowRecordService
    {
        Task<IEnumerable<BorrowRecordDto>> GetAllAsync();
        Task<ServiceResult<BorrowRecordDto>> GetByIdAsync(int id);
        Task<ServiceResult<BorrowRecordDto>> BorrowAsync(BorrowRequestDto dto);
        Task<ServiceResult> ReturnAsync(int id);
    }
}
