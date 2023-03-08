using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.Entities;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Services.Interfaces.Common
{
    public interface INotificationAreaService //: ICommonInterface<NotificationArea>
    {
        Task<IEnumerable<NotificationArea>> GetAll();
        Task<NotificationArea> GetByID(int id);
        Task<NotificationArea> Add(NotificationArea entity);
        Task<NotificationArea> Update(NotificationArea entity);
        Task<bool> Delete(int id);
    }
}
