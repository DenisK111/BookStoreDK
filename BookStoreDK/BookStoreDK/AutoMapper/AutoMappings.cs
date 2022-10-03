using AutoMapper;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;

namespace BookStoreDK.AutoMapper
{
    internal class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<AddAuthorRequest, Author>();
            CreateMap<UpdateAuthorRequest, Author>();

            CreateMap<AddBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();

            CreateMap<AddPersonRequest, Person>();
            CreateMap<UpdatePersonRequest, Person>();
        }
    }
}
