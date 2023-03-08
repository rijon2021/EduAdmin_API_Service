using DotNet.ApplicationCore.DTOs;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using DotNet.Services.Repositories.Interfaces;
using DotNet.Services.Services.Interfaces.Common;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Common;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.Services.Services.Interfaces.Common.AdministrativeUnit;

namespace DotNet.Services.Services.Common.AdministrativeUnit
{
    //GenericRepository<Users>,
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        ResponseMessage rm = new ResponseMessage();
        public CountryService(
            ICountryRepository CountryRepository
            )// : base(dotnetContext)
        {
            _countryRepository = CountryRepository;
        }

        public async Task<IEnumerable<VMCountry>> GetAll()
        {
            return await _countryRepository.GetAll();
        }
        public async Task<VMCountry> GetByID(int id)
        {
            var response = await _countryRepository.GetByID(id);
            return response;
        }
        public async Task<VMCountry> Add(VMCountry country)
        {
            var data = await _countryRepository.Add(country);
            return data;
        }
        public async Task<VMCountry> Update(VMCountry country)
        {
            return await _countryRepository.Update(country);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _countryRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<VMCountry>> GetListByOrganization(List<Organization> objList)
        {
            return await _countryRepository.GetListByOrganization(objList);
        }
        public async Task<IEnumerable<VMCountry>> Search(QueryObject queryObject)
        {
            var data = await _countryRepository.Search(queryObject);
            return data;
        }
    }
}
