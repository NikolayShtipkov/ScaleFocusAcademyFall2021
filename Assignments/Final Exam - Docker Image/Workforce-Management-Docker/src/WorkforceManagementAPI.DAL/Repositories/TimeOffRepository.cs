using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DAL.Repositories
{
    public class TimeOffRepository : ITimeOffRepository
    {
        private readonly DatabaseContext _context;

        public TimeOffRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateTimeOffAsync(TimeOff timeOff)
        {
            await _context.Requests.AddAsync(timeOff);
        }

        public async Task<List<TimeOff>> GetAllAsync()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<TimeOff>> GetMyTimeOffsAsync(string userId)
        {
            var timeOffs = await _context.Requests
                .Where(ci => ci.CreatorId == userId)
                    .ToListAsync();

            return timeOffs;
        }

        public async Task<TimeOff> GetTimeOffAsync(Guid id)
        {
            return await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
        }

        public void DeleteTimeOffAsync(TimeOff timeOff)
        {
            _context.Remove(timeOff);
        }

        public List<TimeOff> GetApprovedTimeOffs(User user)
        {
            return user.Requests
                .Where(r => r.Type == RequestType.Paid && r.Status == Status.Approved)
                .ToList();
        }

        public IEnumerable<User> GetTeamLeadersOutOfOffice(User user)
        {
            return user.Teams
                .Select(t => t.TeamLeader)
                .Where(tl => tl.Requests
                    .Any(r => r.Status == Status.Approved && (r.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= r.EndDate.Date)));
        }
    }
}
