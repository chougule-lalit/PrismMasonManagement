using AutoMapper;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs.DTO;
using PrismMasonManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application
{
    public class PrismMasonManagementAutoMapperProfile : Profile
    {
        public PrismMasonManagementAutoMapperProfile()
        {
            CreateMap<Item,ItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
                .ForMember(dest => dest.LastModificationTime, opt => opt.MapFrom(src => src.LastModificationTime))
                .ForMember(dest => dest.LastModifierId, opt => opt.MapFrom(src => src.LastModifierId));
        }
    }
}
