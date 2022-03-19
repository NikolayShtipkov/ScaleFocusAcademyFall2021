using AutoMapper;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.AutoMapperProfiles
{
    public class OffDaysProfile : Profile
    {
        public OffDaysProfile()
        {
            CreateMap<OffDaysDTO, OffDaysDTO>()
                .ReverseMap();
        }
    }
}
