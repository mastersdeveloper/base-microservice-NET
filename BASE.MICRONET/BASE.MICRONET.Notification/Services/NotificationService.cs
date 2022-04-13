using BASE.MICRONET.Notification.DTOs;
using BASE.MICRONET.Notification.Models;
using BASE.MICRONET.Notification.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BASE.MICRONET.Notification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ContextDatabase _contextDatabase;

        public NotificationService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        public async Task<bool> Add(SendMail sendMailTransaction)
        {
            _contextDatabase.SendMail.Add(sendMailTransaction);
            await _contextDatabase.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SendMailResponse>> GetAll()
        {
            var data = await _contextDatabase.SendMail.ToListAsync();
            var response = new List<SendMailResponse>();

            foreach (var item in data)
            {
                response.Add(new SendMailResponse()
                {
                    AccountId = item.AccountId,
                    Address = item.Address,
                    Message = item.Message,
                    SendDate = item.SendDate,
                    SendMailId = item.SendMailId,
                    Type = item.Type
                });
            }

            return response;
        }
    }
}
