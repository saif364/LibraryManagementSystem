namespace LibraryManagementSystem.API.Dtos
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }

    public class BorrowRequestDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
    }
}
