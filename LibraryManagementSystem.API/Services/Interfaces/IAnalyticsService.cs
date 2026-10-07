using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Dtos;

namespace LibraryManagementSystem.API.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int top);
        Task<IEnumerable<OverdueMemberDto>> GetMembersWithOverdueBooksAsync();
        Task<IEnumerable<BooksByGenreDto>> GetBooksByGenreAsync();
        Task<ServiceResult<MemberBorrowingHistoryDto>> GetMemberBorrowingHistoryAsync(int memberId);
    }
}
