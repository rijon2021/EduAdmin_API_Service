using DotNet.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Infrastructure;

namespace DotNet.Services.Repositories.Interfaces.Common
{

    public interface IGlobalSettingRepository //: ICommonRepository<GlobalSetting>
    {
        Task<IEnumerable<GlobalSetting>> GetAll();
        Task<List<GlobalSetting>> GetAllWithUserOrganization();
        Task<GlobalSetting> GetByID(int id);
        Task<GlobalSetting> Update(GlobalSetting entity);
    }
}
