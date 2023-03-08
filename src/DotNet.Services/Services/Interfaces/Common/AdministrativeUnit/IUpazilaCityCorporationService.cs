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
    public interface IUpazilaCityCorporationService //: ICommonInterface<UpazilaCityCorporation>
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
    }
}
