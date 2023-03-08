using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.Entities;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Services.Interfaces.Common
{
    public interface IUserService //: ICommonInterface<Users>
    {
        ResponseMessage UserAuthentication(AuthUser userResponse);
        Task<IEnumerable<Users>> GetAll();
        Task<Users> GetByID(int id);
        Task<Users> Add(Users entity);
        Task<Users> Update(Users entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<Users>> GetAllByOrganizationID();
        Task<ResponseMessage> GetInitialData();
    }
}
