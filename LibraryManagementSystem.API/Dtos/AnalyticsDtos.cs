namespace LibraryManagementSystem.API.Dtos
{
    public class MostBorrowedBookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }

    public class OverdueMemberDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int OverdueBooksCount { get; set; }
        public List<OverdueBookDto> OverdueBooks { get; set; } = new();
    }

    public class OverdueBookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
    }

    public class BooksByGenreDto
    {
        public int LibraryId { get; set; }
        public string LibraryName { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int BookCount { get; set; }
    }

    public class MemberBorrowingHistoryDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public List<BorrowHistoryItemDto> History { get; set; } = new();
    }

    public class BorrowHistoryItemDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}
