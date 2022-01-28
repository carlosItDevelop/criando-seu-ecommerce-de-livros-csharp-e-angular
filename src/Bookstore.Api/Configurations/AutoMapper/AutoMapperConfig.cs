using APIBookstore.Api.Dtos;
using AutoMapper;
using Bookstore.Domain.Entities;

namespace APIBookstore.Api.Configurations.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
