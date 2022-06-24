using AutoMapper;
using ContainerApp.Contracts.Data.Entities;
using ContainerApp.Contracts.DTO;

namespace ContainerApp.Core.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Item, ItemDTO>();
        }
    }
}
