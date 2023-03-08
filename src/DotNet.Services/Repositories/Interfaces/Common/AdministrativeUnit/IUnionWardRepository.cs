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

    public interface IUnionWardRepository
    {
        Task<IEnumerable<VMUnionWard>> GetAll();
        Task<VMUnionWard> GetByID(int id);
        Task<VMUnionWard> Add(VMUnionWard entity);
        Task<VMUnionWard> Update(VMUnionWard entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMUnionWard>> UpdateOrder(List<VMUnionWard> oList);
        Task<IEnumerable<VMUnionWard>> Search(QueryObject queryObject);
        Task<IEnumerable<VMUnionWard>> GetListByThana(List<VMThana> objList);
        Task<IEnumerable<VMUnionWard>> GetListByOrganizationID(int id);
        Task<bool> SaveOrganizationUnionWardMap(List<OrganizationUnionWardMap> oList);

    }
}
