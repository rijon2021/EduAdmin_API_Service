using DotNet.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Infrastructure;

namespace DotNet.Services.Repositories.Interfaces.Common
{

    public interface IPermissionRepository //: ICommonRepository<Permission>
    {
        Task<IEnumerable<Permission>> GetAll();
        Task<Permission> GetByID(int id);
        Task<Permission> Add(Permission entity);
        Task<Permission> Update(Permission entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<Permission>> UpdateOrder(List<Permission> oList);
    }
}
