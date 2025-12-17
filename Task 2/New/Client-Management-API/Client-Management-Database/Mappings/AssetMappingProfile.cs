using AutoMapper;
using Client_Management_Database.DTOs;
using Client_Management_Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Management_Database.Mappings
{
    internal class AssetMappingProfile : Profile
    {
        public AssetMappingProfile()
        {
            CreateMap<Asset_SearchResult1, AssetDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssetId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.AssetType));

            CreateMap<Asset, AssetDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssetId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.AssetType));
        }
    }
}
