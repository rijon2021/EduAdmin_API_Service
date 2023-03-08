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
    public interface IUserRoleService //: ICommonInterface<UserLevel>
    {
        Task<IEnumerable<UserRole>> GetAll();
        Task<UserRole> GetByID(int id);
        Task<UserRole> Add(UserRole entity);
        Task<UserRole> Update(UserRole entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<UserRole>> UpdateOrder(List<UserRole> oList);
    }
}
