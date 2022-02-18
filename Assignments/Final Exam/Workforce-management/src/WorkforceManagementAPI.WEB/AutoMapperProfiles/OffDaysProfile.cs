using AutoMapper;
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
