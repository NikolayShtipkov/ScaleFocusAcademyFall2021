using AutoMapper;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.AutoMapperProfiles
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, TeamRequestDTO>()
                .ReverseMap();

            CreateMap<Team, TeamResponseDTO>()
                .ReverseMap();
        }


    }
}
