using DotNet.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;

namespace DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit
{

    public interface IThanaRepository
    {
        Task<IEnumerable<VMThana>> GetAll();
        Task<VMThana> GetByID(int id);
        Task<VMThana> Add(VMThana entity);
        Task<VMThana> Update(VMThana entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMThana>> UpdateOrder(List<VMThana> oList);
        Task<IEnumerable<VMThana>> Search(QueryObject queryObject);
        Task<IEnumerable<VMThana>> GetListByUpazilaCityCorporation(List<VMUpazilaCityCorporation> objList); 
        Task<IEnumerable<VMThana>> GetListByOrganizationID(int id);
        Task<bool> SaveOrganizationThanaMap(List<OrganizationThanaMap> oList);


    }
}
