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
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;

        ResponseMessage rm = new ResponseMessage();
        public DistrictService(
            IDistrictRepository DistrictRepository
            )// : base(dotnetContext)
        {
            _districtRepository = DistrictRepository;
        }

        public async Task<IEnumerable<VMDistrict>> GetAll()
        {
            return await _districtRepository.GetAll();
        }
        public async Task<VMDistrict> GetByID(int id)
        {
            var response = await _districtRepository.GetByID(id);
            return response;
        }
        public async Task<VMDistrict> Add(VMDistrict district)
        {
            var data = await _districtRepository.Add(district);
            return data;
        }
        public async Task<VMDistrict> Update(VMDistrict district)
        {
            return await _districtRepository.Update(district);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _districtRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<VMDistrict>> UpdateOrder(List<VMDistrict> oList)
        {
            return await _districtRepository.UpdateOrder(oList);
        }
        public async Task<IEnumerable<VMDistrict>> Search(QueryObject queryObject)
        {
            var data = await _districtRepository.Search(queryObject);
            return data;
        }
        public async Task<IEnumerable<VMDistrict>> GetListByDivision(List<VMDivision> objList)
        {
            return await _districtRepository.GetListByDivision(objList);
        }
        public async Task<IEnumerable<VMDistrict>> GetListByOrganizationID(int id)
        {
            var response = await _districtRepository.GetListByOrganizationID(id);
            return response;
        }

    }
}
