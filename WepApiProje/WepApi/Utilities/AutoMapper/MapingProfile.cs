using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;

namespace WebApi.Utilities.AutoMapper
{
	public class MapingProfile:Profile
	{
        public MapingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();//kaynaktan hedefe(source,destination)
            CreateMap<Book, BookDto>();
            CreateMap<BookDtoForInsertion,Book>();
            CreateMap<UserForRegistrationDto,User>();
        }
    }
}
