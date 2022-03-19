using AutoMapper;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.AutoMapperProfiles
{
    public class TimeOffProfile : Profile
    {
        public TimeOffProfile()
        {
            CreateMap<TimeOff, TimeOffRequestDTO>()
                .ReverseMap();

            CreateMap<TimeOff, TimeOffResponseDTO>()
                .ForMember(t => t.CreatorName , act => act.MapFrom(src => src.Creator.FirstName+" "+src.Creator.LastName))
                .ForMember(t => t.ModifierName, act => act.MapFrom(src => src.Modifier.FirstName+" "+src.Modifier.LastName))
                .ReverseMap();
        }
    }
}
