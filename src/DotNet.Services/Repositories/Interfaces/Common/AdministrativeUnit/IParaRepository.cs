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

    public interface IParaRepository
    {
        Task<IEnumerable<VMPara>> GetAll();
        Task<VMPara> GetByID(int id);
        Task<VMPara> Add(VMPara entity);
        Task<VMPara> Update(VMPara entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMPara>> UpdateOrder(List<VMPara> oList);
        Task<IEnumerable<VMPara>> Search(QueryObject queryObject);
        Task<IEnumerable<VMPara>> GetListByVillageArea(List<VMVillageArea> objList);
        Task<IEnumerable<VMPara>> GetListByOrganizationID(int id);
        Task<bool> SaveOrganizationParaMap(List<OrganizationParaMap> oList);

    }
}
