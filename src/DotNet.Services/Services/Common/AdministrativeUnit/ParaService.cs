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
    public class ParaService : IParaService
    {
        private readonly IParaRepository _paraRepository;

        ResponseMessage rm = new ResponseMessage();
        public ParaService(
            IParaRepository ParaRepository
            )// : base(dotnetContext)
        {
            _paraRepository = ParaRepository;
        }

        public async Task<IEnumerable<VMPara>> GetAll()
        {
            return await _paraRepository.GetAll();
        }
        public async Task<VMPara> GetByID(int id)
        {
            var response = await _paraRepository.GetByID(id);
            return response;
        }
        public async Task<VMPara> Add(VMPara para)
        {
            var data = await _paraRepository.Add(para);
            return data;
        }
        public async Task<VMPara> Update(VMPara para)
        {
            return await _paraRepository.Update(para);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _paraRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<VMPara>> UpdateOrder(List<VMPara> oList)
        {
            return await _paraRepository.UpdateOrder(oList);
        }
        public async Task<IEnumerable<VMPara>> Search(QueryObject queryObject)
        {
            var data = await _paraRepository.Search(queryObject);
            return data;
        }
        public async Task<IEnumerable<VMPara>> GetListByVillageArea(List<VMVillageArea> objList)
        {
            return await _paraRepository.GetListByVillageArea(objList);
        }
        public async Task<IEnumerable<VMPara>> GetListByOrganizationID(int id)
        {
            var response = await _paraRepository.GetListByOrganizationID(id);
            return response;
        }

    }
}
