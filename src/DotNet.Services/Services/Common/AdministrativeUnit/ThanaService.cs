using DotNet.ApplicationCore.DTOs;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using DotNet.Services.Repositories.Interfaces;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Common;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.Services.Services.Interfaces.Common.AdministrativeUnit;
using DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.Services.Repositories.Common.AdministrativeUnit;

namespace DotNet.Services.Services.Common.AdministrativeUnit
{
    public class ThanaService : IThanaService
    {
        private readonly IThanaRepository _thanaRepository;

        ResponseMessage rm = new ResponseMessage();
        public ThanaService(
            IThanaRepository ThanaRepository
            )// : base(dotnetContext)
        {
            _thanaRepository = ThanaRepository;
        }

        public async Task<IEnumerable<VMThana>> GetAll()
        {
            return await _thanaRepository.GetAll();
        }
        public async Task<VMThana> GetByID(int id)
        {
            var response = await _thanaRepository.GetByID(id);
            return response;
        }
        public async Task<VMThana> Add(VMThana thana)
        {
            var data = await _thanaRepository.Add(thana);
            return data;
        }
        public async Task<VMThana> Update(VMThana thana)
        {
            return await _thanaRepository.Update(thana);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _thanaRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<VMThana>> UpdateOrder(List<VMThana> oList)
        {
            return await _thanaRepository.UpdateOrder(oList);
        }
        public async Task<IEnumerable<VMThana>> Search(QueryObject queryObject)
        {
            var data = await _thanaRepository.Search(queryObject);
            return data;
        }
        public async Task<IEnumerable<VMThana>> GetListByUpazilaCityCorporation(List<VMUpazilaCityCorporation> objList)
        {
            return await _thanaRepository.GetListByUpazilaCityCorporation(objList);
        }
        public async Task<IEnumerable<VMThana>> GetListByOrganizationID(int id)
        {
            var response = await _thanaRepository.GetListByOrganizationID(id);
            return response;
        }

    }
}
