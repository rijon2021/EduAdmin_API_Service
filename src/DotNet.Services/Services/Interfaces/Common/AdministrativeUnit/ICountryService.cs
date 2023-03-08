using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Services.Interfaces.Common.AdministrativeUnit
{
    public interface ICountryService //: ICommonInterface<Country>
    {
        Task<IEnumerable<VMCountry>> GetAll();
        Task<VMCountry> GetByID(int id);
        Task<VMCountry> Add(VMCountry entity);
        Task<VMCountry> Update(VMCountry entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMCountry>> GetListByOrganization(List<Organization> objList);
        Task<IEnumerable<VMCountry>> Search(QueryObject queryObject);
    }
}
