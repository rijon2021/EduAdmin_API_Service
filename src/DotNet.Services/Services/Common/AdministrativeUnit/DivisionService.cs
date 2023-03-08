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

namespace DotNet.Services.Services.Common.AdministrativeUnit
{
    //GenericRepository<Users>,
    public class DivisionService : IDivisionService
    {
        private readonly IDivisionRepository _divisionRepository;

        public DivisionService(
            IDivisionRepository DivisionRepository
            )// : base(dotnetContext)
        {
            _divisionRepository = DivisionRepository;
        }

        public async Task<IEnumerable<VMDivision>> GetAll()
        {
            return await _divisionRepository.GetAll();
        }
        public async Task<VMDivision> GetByID(int id)
        {
            var response = await _divisionRepository.GetByID(id);
            return response;
        }
        public async Task<VMDivision> Add(VMDivision division)
        {
            var data = await _divisionRepository.Add(division);
            return data;
        }
        public async Task<VMDivision> Update(VMDivision division)
        {
            return await _divisionRepository.Update(division);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _divisionRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<VMDivision>> UpdateOrder(List<VMDivision> oList)
        {
            return await _divisionRepository.UpdateOrder(oList);
        }
        public async Task<IEnumerable<VMDivision>> Search(QueryObject queryObject)
        {
            var data = await _divisionRepository.Search(queryObject);
            return data;
        }
        //public async Task<IEnumerable<VMDivision>> GetListByOrganization(int id)
        //{
        //    return await _divisionRepository.GetListByOrganization(id);
        //}
        public async Task<IEnumerable<VMDivision>> GetListByCountry(List<VMCountry> objList)
        {
            return await _divisionRepository.GetListByCountry(objList);
        }
        public async Task<IEnumerable<VMDivision>> GetListByOrganizationID(int id)
        {
            var response = await _divisionRepository.GetListByOrganizationID(id);
            return response;
        }

    }
}
