using AutoMapper;
using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Data;
using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Models;
using LibraryManagementSystem.API.Repositories.Interfaces;
using LibraryManagementSystem.API.Services.Interfaces;

namespace LibraryManagementSystem.API.Services
{
    public class BorrowRecordService : IBorrowRecordService
    {
        private readonly LibraryContext _context;
        private readonly IRepository<BorrowRecord> _borrowRecordRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IRepository<Member> _memberRepository;
        private readonly IMapper _mapper;

        public BorrowRecordService(
            LibraryContext context,
            IRepository<BorrowRecord> borrowRecordRepository,
            IBookRepository bookRepository,
            IRepository<Member> memberRepository,
            IMapper mapper)
        {
            _context = context;
            _borrowRecordRepository = borrowRecordRepository;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BorrowRecordDto>> GetAllAsync()
        {
            var records = await _borrowRecordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BorrowRecordDto>>(records);
        }

        public async Task<ServiceResult<BorrowRecordDto>> GetByIdAsync(int id)
        {
            var record = await _borrowRecordRepository.GetByIdAsync(id);
            if (record == null)
            {
                return new ServiceResult<BorrowRecordDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.BorrowRecordNotFound
                };
            }

            return new ServiceResult<BorrowRecordDto>
            {
                Success = true,
                StatusCode = 200,
                Data = _mapper.Map<BorrowRecordDto>(record)
            };
        }

        public async Task<ServiceResult<BorrowRecordDto>> BorrowAsync(BorrowRequestDto dto)
        {
            var book = await _bookRepository.GetByIdAsync(dto.BookId);
            if (book == null)
            {
                return new ServiceResult<BorrowRecordDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.BookNotFound
                };
            }

            var member = await _memberRepository.GetByIdAsync(dto.MemberId);
            if (member == null)
            {
                return new ServiceResult<BorrowRecordDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.MemberNotFound
                };
            }

            if (book.AvailableCopies <= 0)
            {
                return new ServiceResult<BorrowRecordDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = Messages.NoCopiesAvailable
                };
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var record = new BorrowRecord
                {
                    BookId = book.Id,
                    MemberId = member.Id,
                    BorrowDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(14)
                };

                book.AvailableCopies--;

                await _borrowRecordRepository.AddAsync(record);
                _bookRepository.Update(book);
                await _borrowRecordRepository.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ServiceResult<BorrowRecordDto>
                {
                    Success = true,
                    StatusCode = 201,
                    Message = Messages.BookBorrowed,
                    Data = _mapper.Map<BorrowRecordDto>(record)
                };
            }
            catch
            {
                await transaction.RollbackAsync();

                return new ServiceResult<BorrowRecordDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = Messages.BorrowFailed
                };
            }
        }

        public async Task<ServiceResult> ReturnAsync(int id)
        {
            var record = await _borrowRecordRepository.GetByIdAsync(id);
            if (record == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.BorrowRecordNotFound
                };
            }

            if (record.ReturnDate != null)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 400,
                    Message = Messages.AlreadyReturned
                };
            }

            var book = await _bookRepository.GetByIdAsync(record.BookId);
            if (book == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.BookNotFound
                };
            }

            record.ReturnDate = DateTime.UtcNow;
            book.AvailableCopies++;

            _borrowRecordRepository.Update(record);
            _bookRepository.Update(book);
            await _borrowRecordRepository.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = Messages.BookReturned
            };
        }
    }
}
