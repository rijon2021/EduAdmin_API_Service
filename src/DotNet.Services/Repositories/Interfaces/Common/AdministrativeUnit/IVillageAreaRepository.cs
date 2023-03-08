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

    public interface IVillageAreaRepository
    {
        Task<IEnumerable<VMVillageArea>> GetAll();
        Task<VMVillageArea> GetByID(int id);
        Task<VMVillageArea> Add(VMVillageArea entity);
        Task<VMVillageArea> Update(VMVillageArea entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMVillageArea>> UpdateOrder(List<VMVillageArea> oList);
        Task<IEnumerable<VMVillageArea>> Search(QueryObject queryObject);
        Task<IEnumerable<VMVillageArea>> GetListByUnionWard(List<VMUnionWard> objList);
        Task<IEnumerable<VMVillageArea>> GetListByOrganizationID(int id);
        Task<bool> SaveOrganizationVillageAreaMap(List<OrganizationVillageAreaMap> oList);

    }
}
