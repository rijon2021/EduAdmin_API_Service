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

    public interface IDistrictRepository
    {
        Task<IEnumerable<VMDistrict>> GetAll();
        Task<VMDistrict> GetByID(int id);
        Task<VMDistrict> Add(VMDistrict entity);
        Task<VMDistrict> Update(VMDistrict entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMDistrict>> UpdateOrder(List<VMDistrict> oList);
        Task<IEnumerable<VMDistrict>> Search(QueryObject queryObject);
        Task<IEnumerable<VMDistrict>> GetListByDivision(List<VMDivision> objList);
        Task<bool> SaveOrganizationDistrictMap(List<OrganizationDistrictMap> oList);
        Task<IEnumerable<VMDistrict>> GetListByOrganizationID(int id);

    }
}
