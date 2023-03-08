using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Services.Interfaces.Common
{
    public interface IOrganizationAdministrativeUnitMapService //: ICommonInterface<Users>
    {
        Task<ResponseMessage> GetInitialData();
        Task<bool> SaveOrganizationCountryMap(List<OrganizationCountryMap> oList);
        Task<bool> SaveOrganizationDivisionMap(List<OrganizationDivisionMap> oList);
        Task<bool> SaveOrganizationDistrictMap(List<OrganizationDistrictMap> oList);
        Task<bool> SaveOrganizationUpazilaCityCorporationMap(List<OrganizationUpazilaCityCorporationMap> oList);
        Task<bool> SaveOrganizationThanaMap(List<OrganizationThanaMap> oList);
        Task<bool> SaveOrganizationUnionWardMap(List<OrganizationUnionWardMap> oList);
        Task<bool> SaveOrganizationVillageAreaMap(List<OrganizationVillageAreaMap> oList);
        Task<bool> SaveOrganizationParaMap(List<OrganizationParaMap> oList);

    }
}
