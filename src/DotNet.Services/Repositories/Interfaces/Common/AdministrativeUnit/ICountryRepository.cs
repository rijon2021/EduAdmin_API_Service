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

    public interface ICountryRepository //: ICommonRepository<Country>
    {
        Task<IEnumerable<VMCountry>> GetAll();
        Task<VMCountry> GetByID(int id);
        Task<VMCountry> Add(VMCountry entity);
        Task<VMCountry> Update(VMCountry entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMCountry>> Search(QueryObject queryObject);
        Task<IEnumerable<VMCountry>> GetListByOrganization(List<Organization> objList);
        Task<bool> SaveOrganizationCountryMap(List<OrganizationCountryMap> oList);
    }
}
