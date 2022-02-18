using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Contracts
{
    public interface INotificationService
    {
        Task Send(List<User> receiver, string subject, string message);
    }
}
