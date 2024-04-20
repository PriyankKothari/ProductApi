using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.WebApi.ViewModels;

namespace ProductApi.WebApi.Mappers
{
    /// <summary>
    /// Represents auto mapping profile for the request and response view models.
    /// </summary>
    public sealed class ViewModelsMappingProfile : Profile
    {
        /// <summary>
        /// Initiates a new instance of the <see cref="Profile" />.
        /// </summary>
        public ViewModelsMappingProfile()
        {
            CreateMap<ProductRequest, ProductDto>();
            CreateMap<ProductDto, ProductResponse>();
        }
    }
}