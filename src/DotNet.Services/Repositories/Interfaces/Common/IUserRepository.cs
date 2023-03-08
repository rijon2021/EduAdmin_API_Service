using DotNet.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Infrastructure;

namespace DotNet.Services.Repositories.Interfaces.Common
{

    //: IGenericRepository<Users>
    public interface IUserRepository //: ICommonRepository<Users>
    {
        AuthUser UserAuthentication(AuthUser userResponse);
        Task<IEnumerable<Users>> GetAll();
        Task<Users> GetByID(int id);
        Task<Users> Add(Users entity);
        Task<Users> Update(Users entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<Users>> GetAllByOrganizationID();
        //Task<IEnumerable<Users>> GetDivisionListByOrganization(int id);
    }
}
