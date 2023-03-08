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

    public interface IUpazilaCityCorporationRepository
    {
        Task<IEnumerable<VMUpazilaCityCorporation>> GetAll();
        Task<VMUpazilaCityCorporation> GetByID(int id);
        Task<VMUpazilaCityCorporation> Add(VMUpazilaCityCorporation entity);
        Task<VMUpazilaCityCorporation> Update(VMUpazilaCityCorporation entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMUpazilaCityCorporation>> UpdateOrder(List<VMUpazilaCityCorporation> oList);
        Task<IEnumerable<VMUpazilaCityCorporation>> Search(QueryObject queryObject);
        Task<IEnumerable<VMUpazilaCityCorporation>> GetListByDistrict(List<VMDistrict> objList);
        Task<IEnumerable<VMUpazilaCityCorporation>> GetListByOrganizationID(int id);
        Task<bool> SaveOrganizationUpazilaCityCorporationMap(List<OrganizationUpazilaCityCorporationMap> oList);


    }
}
