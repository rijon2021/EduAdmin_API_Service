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
    public class UnionWardService : IUnionWardService
    {
        private readonly IUnionWardRepository _unionWardRepository;

        ResponseMessage rm = new ResponseMessage();
        public UnionWardService(
            IUnionWardRepository UnionWardRepository
            )// : base(dotnetContext)
        {
            _unionWardRepository = UnionWardRepository;
        }

        public async Task<IEnumerable<VMUnionWard>> GetAll()
        {
            return await _unionWardRepository.GetAll();
        }
        public async Task<VMUnionWard> GetByID(int id)
        {
            var response = await _unionWardRepository.GetByID(id);
            return response;
        }
        public async Task<VMUnionWard> Add(VMUnionWard unionWard)
        {
            var data = await _unionWardRepository.Add(unionWard);
            return data;
        }
        public async Task<VMUnionWard> Update(VMUnionWard unionWard)
        {
            return await _unionWardRepository.Update(unionWard);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _unionWardRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<VMUnionWard>> UpdateOrder(List<VMUnionWard> oList)
        {
            return await _unionWardRepository.UpdateOrder(oList);
        }
        public async Task<IEnumerable<VMUnionWard>> Search(QueryObject queryObject)
        {
            var data = await _unionWardRepository.Search(queryObject);
            return data;
        }
        public async Task<IEnumerable<VMUnionWard>> GetListByThana(List<VMThana> objList)
        {
            return await _unionWardRepository.GetListByThana(objList);
        }
        public async Task<IEnumerable<VMUnionWard>> GetListByOrganizationID(int id)
        {
            var response = await _unionWardRepository.GetListByOrganizationID(id);
            return response;
        }

    }
}
