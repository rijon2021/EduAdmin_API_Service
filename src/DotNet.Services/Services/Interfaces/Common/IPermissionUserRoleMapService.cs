using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.DTOs.Common;
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
    public interface IPermissionUserRoleMapService //: ICommonInterface<PermissionUserRoleMap>
    {
        Task<IEnumerable<PermissionUserRoleMap>> GetAll();
        Task<PermissionUserRoleMap> GetByID(int id);
        Task<PermissionUserRoleMap> Add(PermissionUserRoleMap entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMPermissionUserRoleMap>> GetListByUserRoleID(int userRoleID);
        Task<bool> UpdatePermissionList(List<VMPermissionUserRoleMap> oList);
        Task<ResponseMessage> GetInitialData();
    }
}
