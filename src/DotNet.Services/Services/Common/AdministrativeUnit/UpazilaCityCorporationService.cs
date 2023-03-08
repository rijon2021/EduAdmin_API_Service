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
    public class UpazilaCityCorporationService : IUpazilaCityCorporationService
    {
        private readonly IUpazilaCityCorporationRepository _upazilaCityCorporationRepository;

        ResponseMessage rm = new ResponseMessage();
        public UpazilaCityCorporationService(
            IUpazilaCityCorporationRepository UpazilaCityCorporationRepository
            )// : base(dotnetContext)
        {
            _upazilaCityCorporationRepository = UpazilaCityCorporationRepository;
        }

        public async Task<IEnumerable<VMUpazilaCityCorporation>> GetAll()
        {
            return await _upazilaCityCorporationRepository.GetAll();
        }
        public async Task<VMUpazilaCityCorporation> GetByID(int id)
        {
            var response = await _upazilaCityCorporationRepository.GetByID(id);
            return response;
        }
        public async Task<VMUpazilaCityCorporation> Add(VMUpazilaCityCorporation upazilaCityCorporation)
        {
            var data = await _upazilaCityCorporationRepository.Add(upazilaCityCorporation);
            //_dotnetContext.SaveChanges();
            return data;
        }
        public async Task<VMUpazilaCityCorporation> Update(VMUpazilaCityCorporation upazilaCityCorporation)
        {
            return await _upazilaCityCorporationRepository.Update(upazilaCityCorporation);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _upazilaCityCorporationRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<VMUpazilaCityCorporation>> UpdateOrder(List<VMUpazilaCityCorporation> oList)
        {
            return await _upazilaCityCorporationRepository.UpdateOrder(oList);
        }
        public async Task<IEnumerable<VMUpazilaCityCorporation>> Search(QueryObject queryObject)
        {
            var data = await _upazilaCityCorporationRepository.Search(queryObject);
            return data;
        }
        public async Task<IEnumerable<VMUpazilaCityCorporation>> GetListByDistrict(List<VMDistrict> objList)
        {
            return await _upazilaCityCorporationRepository.GetListByDistrict(objList);
        }
        public async Task<IEnumerable<VMUpazilaCityCorporation>> GetListByOrganizationID(int id)
        {
            var response = await _upazilaCityCorporationRepository.GetListByOrganizationID(id);
            return response;
        }

    }
}
