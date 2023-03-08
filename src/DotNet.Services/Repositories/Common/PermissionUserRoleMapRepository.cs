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
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using System.Collections;

namespace DotNet.Services.Repositories.Common
{
    //IGenericRepository<PermissionUserRoleMap>,
    public class PermissionUserRoleMapRepository : IPermissionUserRoleMapRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PermissionUserRoleMapRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PermissionUserRoleMap>> GetAll()
        {
            var permissionUserRoleMaps = _context.PermissionUserRoleMaps.ToList();
            return await Task.FromResult(permissionUserRoleMaps);
        }
        public async Task<PermissionUserRoleMap> GetByID(int id)
        {
            var result = _context.PermissionUserRoleMaps.SingleOrDefault(x => x.PermissionUserRoleMapID == id);
            return await Task.FromResult(result);
        }
        public async Task<PermissionUserRoleMap> Add(PermissionUserRoleMap permissionUserRoleMap)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();

            permissionUserRoleMap.OrganizationID = organizationID;
            permissionUserRoleMap.CreatedBy = Convert.ToInt32(userId);
            permissionUserRoleMap.CreatedDate = DateTime.Now;

            _context.PermissionUserRoleMaps.Add(permissionUserRoleMap);
            _context.SaveChanges();

            return await GetByID(permissionUserRoleMap.PermissionUserRoleMapID);
        }

        //public var GetResult()
        //{
        //    return result;
        //}

        public async Task<IEnumerable<VMPermissionUserRoleMap>> GetListByUserRoleID(int userRoleID)
        {
            List<VMPermissionUserRoleMap> vmMapList = new List<VMPermissionUserRoleMap>();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            var lstPermission = _context.Permissions.ToList();
            var lstMap = _context.PermissionUserRoleMaps.Where(x => x.UserRoleID == userRoleID).ToList();

            if (lstPermission.Count > 0)
            {
                foreach (Permission per in lstPermission)
                {
                    VMPermissionUserRoleMap vmData = new VMPermissionUserRoleMap();
                    vmData.PermissionID = per.PermissionID;
                    vmData.ParentPermissionID = per.ParentPermissionID.Value;
                    vmData.UserRoleID = userRoleID;
                    vmData.OrganizationID = organizationID;
                    vmData.DisplayName = per.DisplayName;
                    vmData.PermissionType = per.PermissionType;
                    vmData.RoutePath = per.RoutePath;
                    vmData.PermissionID = per.PermissionID;
                    if (lstMap.Count > 0)
                    {
                        foreach (PermissionUserRoleMap map in lstMap)
                        {
                            if(per.PermissionID == map.PermissionID)
                            {
                                vmData.PermissionUserRoleMapID = map.PermissionUserRoleMapID;
                                vmData.IsChecked = true;
                            }
                        }
                    }
                    vmMapList.Add(vmData);
                }
            }

            //var result = (
            //       from permissions in lstPermission
            //       join map in lstMap on permissions.PermissionID equals map.PermissionID into join_map

            //       from sup_map in join_map.DefaultIfEmpty()
            //       select new
            //       {
            //           PermissionUserRoleMapID = sup_map.PermissionUserRoleMapID == 0 ? 0 : sup_map.PermissionUserRoleMapID,
            //           PermissionID = permissions.PermissionID,
            //           ParentPermissionID = permissions.ParentPermissionID,
            //           UserRoleID = userRoleID,
            //           OrganizationID = sup_map.OrganizationID == 0 ? 0 : sup_map.OrganizationID,
            //           PermissionName = permissions.PermissionName,
            //           DisplayName = permissions.DisplayName,
            //           PermissionType = permissions.PermissionType,
            //           PermissionTypeStr = permissions.PermissionTypeStr,
            //           RoutePath = permissions.RoutePath
            //       });




            //var returnData = result.ToList();
            return vmMapList;
        }
        public async Task<bool> UpdatePermissionList(List<VMPermissionUserRoleMap> oList)
        {
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();

            if (oList.Count > 0)
            {
                _context.Database.BeginTransaction();
                var delList = oList.Where(x => x.IsChecked == false && x.PermissionUserRoleMapID > 0 && x.OrganizationID == organizationID).ToList();
                foreach (VMPermissionUserRoleMap delItem in delList)
                {
                    var data = _context.PermissionUserRoleMaps.SingleOrDefault(x => x.PermissionUserRoleMapID == delItem.PermissionUserRoleMapID);
                    if (data != null)
                    {
                        _context.Entry(data).State = EntityState.Deleted;
                    }
                }
                var addList = oList.Where(x => x.IsChecked == true && (x.PermissionUserRoleMapID == 0 || x.PermissionUserRoleMapID == null) && x.OrganizationID == organizationID).ToList();
                foreach (VMPermissionUserRoleMap addItem in addList)
                {
                    PermissionUserRoleMap obj = new PermissionUserRoleMap
                    {
                        PermissionID = addItem.PermissionID,
                        UserRoleID = addItem.UserRoleID,
                        OrganizationID = organizationID,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now
                    };
                    _context.PermissionUserRoleMaps.Add(obj);
                }
                _context.Database.CommitTransaction();
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> Delete(int id)
        {
            var data = await GetByID(id);
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

