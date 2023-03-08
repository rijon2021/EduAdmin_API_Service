using DotNet.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Infrastructure;

namespace DotNet.Services.Repositories.Interfaces.Common
{

    public interface INotificationAreaRepository //: ICommonRepository<NotificationArea>
    {
        Task<IEnumerable<NotificationArea>> GetAll();
        Task<NotificationArea> GetByID(int id);
        Task<NotificationArea> Add(NotificationArea entity);
        Task<NotificationArea> Update(NotificationArea entity);
        Task<bool> Delete(int id);
    }
}
