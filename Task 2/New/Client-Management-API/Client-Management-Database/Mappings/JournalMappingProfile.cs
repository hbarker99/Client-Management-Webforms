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
    internal class JournalMappingProfile : Profile
    {
        public JournalMappingProfile()
        {
            CreateMap<Journal, JournalDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.JournalId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (JournalType)src.JournalId))
                .ReverseMap();
        }
    }
}
