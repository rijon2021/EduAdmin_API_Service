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
    //IGenericRepository<Permission>,
    public class PermissionRepository : IPermissionRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PermissionRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Permission>> GetAll()
        {
            var permissions = _context.Permissions.OrderBy(x=>x.OrderNo).ToList();
            return await Task.FromResult(permissions);
        }
        public async Task<Permission> GetByID(int id)
        {
            var result = _context.Permissions.SingleOrDefault(x => x.PermissionID == id);
            return await Task.FromResult(result);
        }
        public async Task<Permission> Add(Permission permission)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            permission.CreatedBy = Convert.ToInt32(userId);
            permission.CreatedDate = DateTime.Now;
            permission.UpdatedBy = Convert.ToInt32(userId);
            permission.UpdatedDate = DateTime.Now;
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            return await GetByID(permission.PermissionID);
        }
        public async Task<Permission> Update(Permission permission)
        {
            var data = await GetByID(permission.PermissionID);
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();

            if (data == null)
            {
                throw new Exception();
            }
            data.PermissionName = permission.PermissionName;
            data.DisplayName = permission.DisplayName;
            data.ParentPermissionID = permission.ParentPermissionID;
            data.IsActive = permission.IsActive;
            data.IconName = permission.IconName;
            data.RoutePath = permission.RoutePath;
            data.PermissionType = permission.PermissionType;
            data.OrderNo = permission.OrderNo;
            data.UpdatedBy = Convert.ToInt32(userId);
            data.UpdatedDate = DateTime.Now;

            _context.Permissions.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
            _context.SaveChanges();
            return data;
        }
        public async Task<bool> Delete(int permissionID)
        {
            var data = await GetByID(permissionID);
            if (data != null)
            {
                _context.Entry(data).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<Permission>> UpdateOrder(List<Permission> oList)
        {
            var dbList = await GetAll();
            foreach (Permission permission in dbList)
            {
                var obj = oList.SingleOrDefault(x => x.PermissionID == permission.PermissionID);
                if (obj != null)
                {
                    permission.OrderNo = obj.OrderNo;
                    _context.Permissions.Attach(permission);
                    _context.Entry(permission).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            var dbList_updated = await GetAll();
            return dbList_updated;
        }
    }
}

