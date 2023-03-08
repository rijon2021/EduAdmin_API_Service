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
    public class VillageAreaService : IVillageAreaService
    {
        private readonly IVillageAreaRepository _villageAreaRepository;

        ResponseMessage rm = new ResponseMessage();
        public VillageAreaService(
            IVillageAreaRepository VillageAreaRepository
            )// : base(dotnetContext)
        {
            _villageAreaRepository = VillageAreaRepository;
        }

        public async Task<IEnumerable<VMVillageArea>> GetAll()
        {
            return await _villageAreaRepository.GetAll();
        }
        public async Task<VMVillageArea> GetByID(int id)
        {
            var response = await _villageAreaRepository.GetByID(id);
            return response;
        }
        public async Task<VMVillageArea> Add(VMVillageArea villageArea)
        {
            var data = await _villageAreaRepository.Add(villageArea);
            return data;
        }
        public async Task<VMVillageArea> Update(VMVillageArea villageArea)
        {
            return await _villageAreaRepository.Update(villageArea);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _villageAreaRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<VMVillageArea>> UpdateOrder(List<VMVillageArea> oList)
        {
            return await _villageAreaRepository.UpdateOrder(oList);
        }
        public async Task<IEnumerable<VMVillageArea>> Search(QueryObject queryObject)
        {
            var data = await _villageAreaRepository.Search(queryObject);
            return data;
        }
        public async Task<IEnumerable<VMVillageArea>> GetListByUnionWard(List<VMUnionWard> objList)
        {
            return await _villageAreaRepository.GetListByUnionWard(objList);
        }
        public async Task<IEnumerable<VMVillageArea>> GetListByOrganizationID(int id)
        {
            var response = await _villageAreaRepository.GetListByOrganizationID(id);
            return response;
        }
    }
}
