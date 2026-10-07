using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Data;
using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.API.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly LibraryContext _context;

        public AnalyticsService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int top)
        {
            var counts = await _context.BorrowRecords
                .GroupBy(br => br.BookId)
                .Select(g => new { BookId = g.Key, BorrowCount = g.Count() })
                .OrderByDescending(g => g.BorrowCount)
                .Take(top)
                .ToListAsync();

            var bookIds = counts.Select(c => c.BookId).ToList();
            var books = await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            var result = new List<MostBorrowedBookDto>();

            foreach (var count in counts)
            {
                var book = books.First(b => b.Id == count.BookId);

                result.Add(new MostBorrowedBookDto
                {
                    BookId = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    BorrowCount = count.BorrowCount
                });
            }

            return result;
        }

        public async Task<IEnumerable<OverdueMemberDto>> GetMembersWithOverdueBooksAsync()
        {
            var today = DateTime.UtcNow;

            var overdueRecords = await _context.BorrowRecords
                .Include(br => br.Member)
                .Include(br => br.Book)
                .Where(br => br.ReturnDate == null && br.DueDate < today)
                .ToListAsync();

            var result = overdueRecords
                .GroupBy(br => br.Member!)
                .Select(g => new OverdueMemberDto
                {
                    MemberId = g.Key.Id,
                    MemberName = $"{g.Key.Name} {g.Key.LastName}",
                    Email = g.Key.Email,
                    OverdueBooksCount = g.Count(),
                    OverdueBooks = g.Select(br => new OverdueBookDto
                    {
                        BookId = br.BookId,
                        Title = br.Book!.Title,
                        DueDate = br.DueDate,
                        DaysOverdue = (today - br.DueDate).Days
                    }).ToList()
                })
                .ToList();

            return result;
        }

        public async Task<IEnumerable<BooksByGenreDto>> GetBooksByGenreAsync()
        {
            var grouped = await _context.Books
                .GroupBy(b => new { b.LibraryId, b.Genre })
                .Select(g => new { g.Key.LibraryId, g.Key.Genre, BookCount = g.Count() })
                .ToListAsync();

            var libraries = await _context.Libraries.ToListAsync();

            var result = grouped
                .Select(g => new BooksByGenreDto
                {
                    LibraryId = g.LibraryId,
                    LibraryName = libraries.First(l => l.Id == g.LibraryId).Name,
                    Genre = g.Genre,
                    BookCount = g.BookCount
                })
                .OrderBy(b => b.LibraryName)
                .ThenBy(b => b.Genre)
                .ToList();

            return result;
        }

        public async Task<ServiceResult<MemberBorrowingHistoryDto>> GetMemberBorrowingHistoryAsync(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                return new ServiceResult<MemberBorrowingHistoryDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.MemberNotFound
                };
            }

            var history = await _context.BorrowRecords
                .Include(br => br.Book)
                .Where(br => br.MemberId == memberId)
                .OrderByDescending(br => br.BorrowDate)
                .Select(br => new BorrowHistoryItemDto
                {
                    BookId = br.BookId,
                    Title = br.Book!.Title,
                    BorrowDate = br.BorrowDate,
                    DueDate = br.DueDate,
                    ReturnDate = br.ReturnDate,
                    IsReturned = br.ReturnDate != null
                })
                .ToListAsync();

            return new ServiceResult<MemberBorrowingHistoryDto>
            {
                Success = true,
                StatusCode = 200,
                Data = new MemberBorrowingHistoryDto
                {
                    MemberId = member.Id,
                    MemberName = $"{member.Name} {member.LastName}",
                    History = history
                }
            };
        }
    }
}
