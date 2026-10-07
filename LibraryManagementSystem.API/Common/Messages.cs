namespace LibraryManagementSystem.API.Common
{
    public static class Messages
    {
        public const string IdMismatch = "The id in the URL and the id in the body do not match.";

        public const string BookNotFound = "Book not found.";
        public const string BookAlreadyExists = "A book with this ISBN already exists.";
        public const string BookCreated = "Book added successfully.";
        public const string BookUpdated = "Book updated successfully.";
        public const string BookDeleted = "Book deleted successfully.";
        public const string NoCopiesAvailable = "No copies of this book are available right now.";

        public const string MemberNotFound = "Member not found.";
        public const string MemberCreated = "Member added successfully.";
        public const string MemberUpdated = "Member updated successfully.";
        public const string MemberDeleted = "Member deleted successfully.";

        public const string LibraryNotFound = "Library not found.";
        public const string LibraryCreated = "Library added successfully.";
        public const string LibraryUpdated = "Library updated successfully.";
        public const string LibraryDeleted = "Library deleted successfully.";

        public const string BorrowRecordNotFound = "Borrow record not found.";
        public const string BookBorrowed = "Book borrowed successfully.";
        public const string BookReturned = "Book returned successfully.";
        public const string AlreadyReturned = "This book is already returned.";
        public const string BorrowFailed = "Something went wrong while borrowing the book. Please try again.";
    }
}
