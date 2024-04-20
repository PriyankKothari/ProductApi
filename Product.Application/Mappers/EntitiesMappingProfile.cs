using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Mappers
{
    /// <summary>
    /// Represents auto mapping profile for the business entities.
    /// </summary>
    public sealed class EntitiesMappingProfile : Profile
    {
        /// <summary>
        /// Initiates a new instance of the <see cref="Profile" />.
        /// </summary>
        public EntitiesMappingProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}