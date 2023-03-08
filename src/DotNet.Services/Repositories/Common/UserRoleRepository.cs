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
    //IGenericRepository<userRole>,
    public class UserRoleRepository : IUserRoleRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserRoleRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserRole>> GetAll()
        {
            var result = _context.UserRoles.OrderBy(x => x.OrderNo).ToList();
            return await Task.FromResult(result);
        }
        public async Task<UserRole> GetByID(int id)
        {
            var result = _context.UserRoles.SingleOrDefault(x => x.UserRoleID == id);
            return await Task.FromResult(result);
        }
        public async Task<UserRole> Add(UserRole userRole)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            userRole.CreatedBy = userId;
            userRole.CreatedDate = DateTime.Now;
            _context.UserRoles.Add(userRole);
            _context.SaveChanges();

            return await GetByID(userRole.UserRoleID);
        }
        public async Task<UserRole> Update(UserRole userRole)
        {
            var data = await GetByID(userRole.UserRoleID);
            if (data == null)
            {
                throw new Exception();
            }
            data.UserRoleName = userRole.UserRoleName;
            data.IsActive = userRole.IsActive;
            data.OrderNo = userRole.OrderNo;

            _context.UserRoles.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
            _context.SaveChanges();
            return data;
        }
        public async Task<bool> Delete(int userRoleID)
        {
            var data = await GetByID(userRoleID);
            if (data != null)
            {
                //if(data.IsActive == true)
                //{
                //    throw new Exception("Active Role can not delete");
                //}
                _context.Entry(data).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<UserRole>> UpdateOrder(List<UserRole> oList)
        {
            var dbList = await GetAll();
            foreach(UserRole userRole in dbList)
            {
                var obj = oList.SingleOrDefault(x => x.UserRoleID == userRole.UserRoleID);
                if(obj != null)
                {
                    userRole.OrderNo = obj.OrderNo;
                    _context.UserRoles.Attach(userRole);
                    _context.Entry(userRole).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            var dbList_updated = await GetAll();
            return dbList_updated;
        }
    }
}

