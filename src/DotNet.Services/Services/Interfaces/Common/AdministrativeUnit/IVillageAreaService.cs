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
    public interface IVillageAreaService //: ICommonInterface<VillageArea>
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
    }
}
