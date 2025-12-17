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
    internal class LiabilityMappingProfile : Profile
    {
        public LiabilityMappingProfile() {
            CreateMap<Liability, LiabilityDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LiabilityId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.LiabilityType));
        }
    }
}
