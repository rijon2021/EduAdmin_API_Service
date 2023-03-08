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
    public interface IParaService //: ICommonInterface<Para>
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
    }
}
