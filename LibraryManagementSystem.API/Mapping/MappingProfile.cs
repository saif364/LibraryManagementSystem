using AutoMapper;
using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Models;

namespace LibraryManagementSystem.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();

            CreateMap<Member, MemberDto>();
            CreateMap<CreateMemberDto, Member>();
            CreateMap<UpdateMemberDto, Member>();

            CreateMap<Library, LibraryDto>();
            CreateMap<CreateLibraryDto, Library>();
            CreateMap<UpdateLibraryDto, Library>();

            CreateMap<BorrowRecord, BorrowRecordDto>();
        }
    }
}
