using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL.Contracts
{
    public interface ITimeOffRepository
    {
        Task CreateTimeOffAsync(TimeOff timeOff);
        void DeleteTimeOffAsync(TimeOff timeOff);
        Task<List<TimeOff>> GetAllAsync();
        List<TimeOff> GetApprovedTimeOffs(User user);
        Task<List<TimeOff>> GetMyTimeOffsAsync(string userId);
        IEnumerable<User> GetTeamLeadersOutOfOffice(User user);
        Task<TimeOff> GetTimeOffAsync(Guid id);
        Task SaveChangesAsync();
    }
}