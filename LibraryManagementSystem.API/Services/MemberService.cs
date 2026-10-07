using AutoMapper;
using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Models;
using LibraryManagementSystem.API.Repositories.Interfaces;
using LibraryManagementSystem.API.Services.Interfaces;

namespace LibraryManagementSystem.API.Services
{
    public class MemberService : IMemberService
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IMapper _mapper;

        public MemberService(IRepository<Member> memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDto>> GetAllAsync()
        {
            var members = await _memberRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MemberDto>>(members);
        }

        public async Task<ServiceResult<MemberDto>> GetByIdAsync(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
            {
                return new ServiceResult<MemberDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.MemberNotFound
                };
            }

            return new ServiceResult<MemberDto>
            {
                Success = true,
                StatusCode = 200,
                Data = _mapper.Map<MemberDto>(member)
            };
        }

        public async Task<ServiceResult<MemberDto>> CreateAsync(CreateMemberDto dto)
        {
            var member = _mapper.Map<Member>(dto);
            member.MembershipDate = DateTime.UtcNow;

            await _memberRepository.AddAsync(member);
            await _memberRepository.SaveChangesAsync();

            return new ServiceResult<MemberDto>
            {
                Success = true,
                StatusCode = 201,
                Message = Messages.MemberCreated,
                Data = _mapper.Map<MemberDto>(member)
            };
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateMemberDto dto)
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

            var existing = await _memberRepository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.MemberNotFound
                };
            }

            _mapper.Map(dto, existing);

            _memberRepository.Update(existing);
            await _memberRepository.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = Messages.MemberUpdated
            };
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var existing = await _memberRepository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 404,
                    Message = Messages.MemberNotFound
                };
            }

            _memberRepository.Delete(existing);
            await _memberRepository.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = Messages.MemberDeleted
            };
        }
    }
}
