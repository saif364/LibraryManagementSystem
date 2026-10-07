using LibraryManagementSystem.API.Models;

namespace LibraryManagementSystem.API.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByIsbnAsync(string isbn);
    }
}
