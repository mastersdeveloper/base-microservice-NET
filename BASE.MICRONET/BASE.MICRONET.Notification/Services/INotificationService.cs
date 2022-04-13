using BASE.MICRONET.Notification.DTOs;
using BASE.MICRONET.Notification.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BASE.MICRONET.Notification.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<SendMailResponse>> GetAll();

        Task<bool> Add(SendMail sendMailTransaction);
    }
}
