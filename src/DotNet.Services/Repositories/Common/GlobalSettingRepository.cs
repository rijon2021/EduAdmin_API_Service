using DotNet.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Http;
using DotNet.ApplicationCore.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Infrastructure.Persistence.Contexts;
using AutoMapper;
using DotNet.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Repositories.Interfaces;
using DotNet.ApplicationCore.Utils.Enum;
using Microsoft.EntityFrameworkCore;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DotNet.Services.Repositories.Common
{
    //IGenericRepository<GlobalSetting>,
    public class GlobalSettingRepository : IGlobalSettingRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GlobalSettingRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GlobalSetting>> GetAll()
        {
            var GlobalSettings = _context.GlobalSettings.ToList();
            return await Task.FromResult(GlobalSettings);
        }
        public async Task<List<GlobalSetting>> GetAllWithUserOrganization()
        {
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            var globalSettings = _context.GlobalSettings.ToList();

            //var GlobalSettings = _context.GlobalSettings.Where(x=>x.IsActive == true && (x.OrganizationID == organizationID || x.OrganizationID == 0)).ToList();
            return await Task.FromResult(globalSettings);
        }
        public async Task<GlobalSetting> GetByID(int id)
        {
            var result = _context.GlobalSettings.SingleOrDefault(x => x.GlobalSettingID == id);
            return await Task.FromResult(result);
        }
        public async Task<GlobalSetting> Add(GlobalSetting globalSetting)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            globalSetting.CreatedBy = Convert.ToInt32(userId);
            globalSetting.CreatedDate = DateTime.Now;
            globalSetting.UpdatedBy = Convert.ToInt32(userId);
            globalSetting.UpdatedDate = DateTime.Now;
            _context.GlobalSettings.Add(globalSetting);
            _context.SaveChanges();

            return await GetByID(globalSetting.GlobalSettingID);
        }
        public async Task<GlobalSetting> Update(GlobalSetting globalSetting)
        {
            var data = await GetByID(globalSetting.GlobalSettingID);
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();

            if (data == null)
            {
                throw new Exception();
            }
            data.GlobalSettingName = globalSetting.GlobalSettingName;
            data.Value = globalSetting.Value;
            data.ValueInString = globalSetting.ValueInString;
            data.IsActive = globalSetting.IsActive;
            data.OrganizationID = globalSetting.OrganizationID;

            //if (data.GlobalSettingID == (int)GlobalSettingsEnum.Google_Map_Key)
            //{
            //    var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //    data.ValueInString = EncryptionDecryptionUsingSymmetricKey.EncryptString(key, data.ValueInString.Trim());
            //}
            _context.GlobalSettings.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
            _context.SaveChanges();
            return data;
        }
        public async Task<bool> Delete(int globalSettingID)
        {
            var data = await GetByID(globalSettingID);
            if (data != null)
            {
                _context.Entry(data).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}

