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
    public interface IOrganizaionService //: ICommonInterface<UserRole>
    {
        Task<IEnumerable<Organization>> GetAll();
        Task<Organization> GetByID(int id);
        Task<Organization> Add(Organization entity);
        Task<Organization> Update(Organization entity);
        Task<bool> Delete(int id);
        Task<ResponseMessage> GetInitialData();
    }
}
