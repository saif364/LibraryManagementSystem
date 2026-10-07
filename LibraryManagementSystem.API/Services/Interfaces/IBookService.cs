using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Dtos;

namespace LibraryManagementSystem.API.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<ServiceResult<BookDto>> GetByIdAsync(int id);
        Task<ServiceResult<BookDto>> CreateAsync(CreateBookDto dto);
        Task<ServiceResult> UpdateAsync(int id, UpdateBookDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
