using DotNet.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Infrastructure;
using System.Collections;

namespace DotNet.Services.Repositories.Interfaces.Common
{

    public interface IPermissionUserRoleMapRepository //: ICommonRepository<PermissionUserRoleMap>
    {
        Task<IEnumerable<PermissionUserRoleMap>> GetAll();
        Task<PermissionUserRoleMap> GetByID(int id);
        Task<PermissionUserRoleMap> Add(PermissionUserRoleMap entity);
        Task<IEnumerable<VMPermissionUserRoleMap>> GetListByUserRoleID(int userRoleID);
        Task<bool> UpdatePermissionList(List<VMPermissionUserRoleMap> oList);
        Task<bool> Delete(int id);

    }
}
