using DotNet.ApplicationCore.DTOs;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using DotNet.Services.Repositories.Interfaces;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Services.Interfaces.Common;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using DotNet.Services.Repositories.Common;
using DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using DotNet.Services.Services.Interfaces.Common.AdministrativeUnit;
using DotNet.Services.Repositories.Common.AdministrativeUnit;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;

namespace DotNet.Services.Services.Common
{
    //GenericRepository<Users>,
    public class OrganizationAdministrativeUnitMapService : IOrganizationAdministrativeUnitMapService
    {
        private readonly IOrganizationRepository _organizaionRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly IDivisionRepository divisionRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IUpazilaCityCorporationRepository _upazilaCityCorporationRepository;
        private readonly IThanaRepository _thanahanaRepository;
        private readonly IUnionWardRepository _unionWardRepository;
        private readonly IVillageAreaRepository _villageAreaRepository;
        private readonly IParaRepository _paraRepository;


        ResponseMessage rm = new ResponseMessage();
        public OrganizationAdministrativeUnitMapService(
            IOrganizationRepository organizaionRepository,
            ICountryRepository countryRepository,
            IDivisionRepository divisionRepository,
            IDistrictRepository districtRepository,
            IUpazilaCityCorporationRepository upazilaCityCorporationRepository,
            IThanaRepository thanahanaRepository,
            IUnionWardRepository unionWardRepository,
            IVillageAreaRepository villageAreaRepository,
            IParaRepository paraRepository
            )// : base(dotnetContext)
        {
            _organizaionRepository = organizaionRepository;
            _countryRepository = countryRepository;
            _divisionRepository = divisionRepository;
            _districtRepository = districtRepository;
            _upazilaCityCorporationRepository = upazilaCityCorporationRepository;
            _thanahanaRepository = thanahanaRepository;
            _unionWardRepository = unionWardRepository;
            _villageAreaRepository = villageAreaRepository;
            _paraRepository = paraRepository;
        }

        public async Task<ResponseMessage> GetInitialData()
        {
            try
            {
                var lstOrganization = await _organizaionRepository.GetAll();
                var lstCountry = await _countryRepository.GetAll();
                rm.StatusCode = ReturnStatus.Success;
                rm.ResponseObj = new
                {
                    lstOrganization,
                    lstCountry
                };
            }
            catch (Exception ex)
            {
                rm.Message = ex.Message;
                rm.StatusCode = ReturnStatus.Failed;
            }
            return rm;
        }
        public async Task<bool> SaveOrganizationCountryMap(List<OrganizationCountryMap> oList)
        {
            var response = await _countryRepository.SaveOrganizationCountryMap(oList);
            return response;
        }
        public async Task<bool> SaveOrganizationDivisionMap(List<OrganizationDivisionMap> oList)
        {
            var response = await _divisionRepository.SaveOrganizationDivisionMap(oList);
            return response;
        }
        public async Task<bool> SaveOrganizationDistrictMap(List<OrganizationDistrictMap> oList)
        {
            var response = await _districtRepository.SaveOrganizationDistrictMap(oList);
            return response;
        }
        public async Task<bool> SaveOrganizationUpazilaCityCorporationMap(List<OrganizationUpazilaCityCorporationMap> oList)
        {
            var response = await _upazilaCityCorporationRepository.SaveOrganizationUpazilaCityCorporationMap(oList);
            return response;
        }
        public async Task<bool> SaveOrganizationThanaMap(List<OrganizationThanaMap> oList)
        {
            var response = await _thanahanaRepository.SaveOrganizationThanaMap(oList);
            return response;
        }
        public async Task<bool> SaveOrganizationUnionWardMap(List<OrganizationUnionWardMap> oList)
        {
            var response = await _unionWardRepository.SaveOrganizationUnionWardMap(oList);
            return response;
        }
        public async Task<bool> SaveOrganizationVillageAreaMap(List<OrganizationVillageAreaMap> oList)
        {
            var response = await _villageAreaRepository.SaveOrganizationVillageAreaMap(oList);
            return response;
        }
        public async Task<bool> SaveOrganizationParaMap(List<OrganizationParaMap> oList)
        {
            var response = await _paraRepository.SaveOrganizationParaMap(oList);
            return response;
        }
    }
}
