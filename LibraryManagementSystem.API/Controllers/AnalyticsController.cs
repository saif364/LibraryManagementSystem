using LibraryManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("most-borrowed-books")]
        public async Task<IActionResult> GetMostBorrowedBooks(int top = 10)
        {
            var books = await _analyticsService.GetMostBorrowedBooksAsync(top);
            return Ok(books);
        }

        [HttpGet("overdue-members")]
        public async Task<IActionResult> GetMembersWithOverdueBooks()
        {
            var members = await _analyticsService.GetMembersWithOverdueBooksAsync();
            return Ok(members);
        }

        [HttpGet("books-by-genre")]
        public async Task<IActionResult> GetBooksByGenre()
        {
            var books = await _analyticsService.GetBooksByGenreAsync();
            return Ok(books);
        }

        [HttpGet("members/{id}/borrowing-history")]
        public async Task<IActionResult> GetMemberBorrowingHistory(int id)
        {
            var result = await _analyticsService.GetMemberBorrowingHistoryAsync(id);

            if (!result.Success)
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });

            return StatusCode(result.StatusCode, new { status = result.StatusCode, data = result.Data });
        }
    }
}
