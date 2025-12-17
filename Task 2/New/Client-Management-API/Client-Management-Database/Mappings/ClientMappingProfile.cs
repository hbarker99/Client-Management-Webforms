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
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<Client_SearchResult1, Client_ReadResult>();

            CreateMap<Client_ReadResult, ClientInfoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (ClientStatus)(src.StatusLookupValueId ?? 0)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LastName + ", " + src.FirstName));

            CreateMap<Client_SearchResult1, ClientInfoDTO>()
                .ConvertUsing((entity, c, context) =>
                {
                    var intermediate = context.Mapper.Map<Client_ReadResult>(entity);
                    return context.Mapper.Map<ClientInfoDTO>(intermediate);
                });

            CreateMap<Client, ClientDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (ClientStatus)(src.StatusLookupValueId ?? 0)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LastName + ", " + src.FirstName));
        }
    }
}
