using AutoMapper;
using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Models;
using LibraryManagementSystem.API.Repositories.Interfaces;
using LibraryManagementSystem.API.Services.Interfaces;

namespace LibraryManagementSystem.API.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<ServiceResult<BookDto>> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Book id {BookId} not found", id);

                return new ServiceResult<BookDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.BookNotFound
                };
            }

            return new ServiceResult<BookDto>
            {
                Success = true,
                StatusCode = 200,
                Data = _mapper.Map<BookDto>(book)
            };
        }

        public async Task<ServiceResult<BookDto>> CreateAsync(CreateBookDto dto)
        {
            var existing = await _bookRepository.GetByIsbnAsync(dto.ISBN);
            if (existing != null)
            {
                _logger.LogWarning(" isbn {Isbn} already there", dto.ISBN);

                return new ServiceResult<BookDto>
                {
                    Success = false,
                    StatusCode = 409,
                    Message = Messages.BookAlreadyExists
                };
            }

            var book = _mapper.Map<Book>(dto);
            book.AvailableCopies = book.TotalCopies;

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            _logger.LogInformation("book {BookId} created with isbn {Isbn}", book.Id, book.ISBN);

            return new ServiceResult<BookDto>
            {
                Success = true,
                StatusCode = 201,
                Message = Messages.BookCreated,
                Data = _mapper.Map<BookDto>(book)
            };
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateBookDto dto)
        {
            if (id != dto.Id)
            {
                _logger.LogWarning($"Id wrong: route id {id}, body id {dto.Id}");

                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 400,
                    Message = Messages.IdMismatch
                };
            }

            var existing = await _bookRepository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Book id {BookId} not found", id);

                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.BookNotFound
                };
            }

            _mapper.Map(dto, existing);

            _bookRepository.Update(existing);
            await _bookRepository.SaveChangesAsync();

            _logger.LogInformation("Book data {BookId} updated", id);

            return new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = Messages.BookUpdated
            };
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var existing = await _bookRepository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Book {BookId} not found", id);

                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.BookNotFound
                };
            }

            _bookRepository.Delete(existing);
            await _bookRepository.SaveChangesAsync();

            _logger.LogInformation("Book {BookId} deleted", id);

            return new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = Messages.BookDeleted
            };
        }
    }
}
