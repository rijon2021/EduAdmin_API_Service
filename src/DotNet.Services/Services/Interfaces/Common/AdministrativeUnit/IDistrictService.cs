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
    public interface IDistrictService //: ICommonInterface<District>
    {
        Task<IEnumerable<VMDistrict>> GetAll();
        Task<VMDistrict> GetByID(int id);
        Task<VMDistrict> Add(VMDistrict entity);
        Task<VMDistrict> Update(VMDistrict entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMDistrict>> UpdateOrder(List<VMDistrict> oList);
        Task<IEnumerable<VMDistrict>> Search(QueryObject queryObject);
        Task<IEnumerable<VMDistrict>> GetListByDivision(List<VMDivision> objList);
        Task<IEnumerable<VMDistrict>> GetListByOrganizationID(int id);
    }
}
