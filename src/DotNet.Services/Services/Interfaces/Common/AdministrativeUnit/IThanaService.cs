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
    public interface IThanaService //: ICommonInterface<Thana>
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

    }
}
