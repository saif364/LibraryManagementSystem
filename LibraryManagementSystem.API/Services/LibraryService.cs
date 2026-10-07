using AutoMapper;
using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Models;
using LibraryManagementSystem.API.Repositories.Interfaces;
using LibraryManagementSystem.API.Services.Interfaces;

namespace LibraryManagementSystem.API.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IRepository<Library> _libraryRepository;
        private readonly IMapper _mapper;

        public LibraryService(IRepository<Library> libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LibraryDto>> GetAllAsync()
        {
            var libraries = await _libraryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LibraryDto>>(libraries);
        }

        public async Task<ServiceResult<LibraryDto>> GetByIdAsync(int id)
        {
            var library = await _libraryRepository.GetByIdAsync(id);
            if (library == null)
            {
                return new ServiceResult<LibraryDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.LibraryNotFound
                };
            }

            return new ServiceResult<LibraryDto>
            {
                Success = true,
                StatusCode = 200,
                Data = _mapper.Map<LibraryDto>(library)
            };
        }

        public async Task<ServiceResult<LibraryDto>> CreateAsync(CreateLibraryDto dto)
        {
            var library = _mapper.Map<Library>(dto);

            await _libraryRepository.AddAsync(library);
            await _libraryRepository.SaveChangesAsync();

            return new ServiceResult<LibraryDto>
            {
                Success = true,
                StatusCode = 201,
                Message = Messages.LibraryCreated,
                Data = _mapper.Map<LibraryDto>(library)
            };
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateLibraryDto dto)
        {
            if (id != dto.Id)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 400,
                    Message = Messages.IdMismatch
                };
            }

            var existing = await _libraryRepository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.LibraryNotFound
                };
            }

            _mapper.Map(dto, existing);

            _libraryRepository.Update(existing);
            await _libraryRepository.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = Messages.LibraryUpdated
            };
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var existing = await _libraryRepository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.LibraryNotFound
                };
            }

            _libraryRepository.Delete(existing);
            await _libraryRepository.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = Messages.LibraryDeleted
            };
        }
    }
}
