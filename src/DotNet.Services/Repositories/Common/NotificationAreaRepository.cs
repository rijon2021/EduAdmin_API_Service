using DotNet.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Http;
using DotNet.ApplicationCore.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Infrastructure.Persistence.Contexts;
using AutoMapper;
using DotNet.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Repositories.Interfaces;
using DotNet.ApplicationCore.Utils.Enum;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Services.Repositories.Common
{
    //IGenericRepository<NotificationArea>,
    public class NotificationAreaRepository : INotificationAreaRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public NotificationAreaRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<IEnumerable<NotificationArea>> GetAll()
        {
            var NotificationAreas = _context.NotificationAreas.ToList();
            return await Task.FromResult(NotificationAreas);
        }
        public async Task<NotificationArea> GetByID(int id)
        {
            var result = _context.NotificationAreas.SingleOrDefault(x => x.NotificationAreaID == id);
            return await Task.FromResult(result);
        }
        public async Task<NotificationArea> Add(NotificationArea notificationArea)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            _context.NotificationAreas.Add(notificationArea);
            _context.SaveChanges();

            return await GetByID(notificationArea.NotificationAreaID);
        }
        public async Task<NotificationArea> Update(NotificationArea notificationArea)
        {
            var data = await GetByID(notificationArea.NotificationAreaID);
            if (data == null)
            {
                throw new Exception();
            }
            data.NotificationAreaName = notificationArea.NotificationAreaName;
            data.NotificationType = notificationArea.NotificationType;
            data.NotificationBody = notificationArea.NotificationBody;
            data.IsActive = notificationArea.IsActive;
            data.ExpireTime = notificationArea.ExpireTime;

            _context.NotificationAreas.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
            _context.SaveChanges();
            return data;
        }
        public async Task<bool> Delete(int notificationAreaID)
        {
            var data = await GetByID(notificationAreaID);
            if (data != null)
            {
                _context.Entry(data).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}

