using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.Entities;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Services.Interfaces.Common
{
    public interface IGlobalSettingService //: ICommonInterface<GlobalSetting>
    {
        Task<IEnumerable<GlobalSetting>> GetAll();
        Task<List<GlobalSetting>> GetAllWithUserOrganization();
        Task<GlobalSetting> GetByID(int id);
        //Task<GlobalSetting> Add(GlobalSetting entity);
        Task<GlobalSetting> Update(GlobalSetting entity);
        //Task<bool> Delete(int id);
        //Task<IEnumerable<GlobalSetting>> UpdateOrder(List<GlobalSetting> oList);
    }
}
