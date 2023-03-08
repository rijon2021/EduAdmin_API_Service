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
    public interface IUnionWardService //: ICommonInterface<UnionWard>
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
    }
}
