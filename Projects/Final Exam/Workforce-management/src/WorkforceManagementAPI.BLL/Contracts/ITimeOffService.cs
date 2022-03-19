using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.BLL.Contracts
{
    public interface ITimeOffService
    {
        Task<bool> CreateTimeOffAsync(TimeOffRequestDTO timoffRequest, string creatorId);

        Task<List<TimeOff>> GetAllAsync();

        Task<List<TimeOff>> GetMyTimeOffs(string userId);

        Task<TimeOff> GetTimeOffAsync(Guid id);

        Task<bool> DeleteTimeOffAsync(Guid id);

        Task<bool> EditTimeOffAsync(Guid id, EditTimeOffRequestDTO timoffRequest,User modifier);

        Task<bool> SubmitFeedbackForTimeOffRequestAsync(User user, Guid timeOffId, Status status);

        OffDaysDTO GetOffDays(User user);
    }
}
