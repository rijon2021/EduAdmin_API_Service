using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.DTOs.VM;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using DotNet.Services.Services.Common;
using DotNet.Services.Services.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace DotNet.WebApi.Controllers.Common
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationAdministrativeUnitMapController : ControllerBase
    {
        private readonly IOrganizationAdministrativeUnitMapService _organizationAdministrativeUnitMapService;
        private readonly ILogger<OrganizationAdministrativeUnitMapController> _logger;

        public OrganizationAdministrativeUnitMapController(
            IOrganizationAdministrativeUnitMapService organizationAdministrativeUnitMapService,
            ILogger<OrganizationAdministrativeUnitMapController> logger
            )
        {
            _organizationAdministrativeUnitMapService = organizationAdministrativeUnitMapService;
            _logger = logger;
        }

        [HttpGet]
        [Route("getInitialData")]
        public async Task<IActionResult> GetInitialData()
        {
            var response = await _organizationAdministrativeUnitMapService.GetInitialData();
            return Ok(response);
        }
        [HttpPost]
        [Route("saveOrganizationCountryMap")]
        public async Task<IActionResult> SaveOrganizationCountryMap(RequestMessage rm)
        {
            var list = JsonConvert.DeserializeObject<List<OrganizationCountryMap>>(rm.RequestObj.ToString());
            var response = await _organizationAdministrativeUnitMapService.SaveOrganizationCountryMap(list);
            return Ok(response);
        }
        [HttpPost]
        [Route("saveOrganizationDivisionMap")]
        public async Task<IActionResult> SaveOrganizationDivisionMap(RequestMessage rm)
        {
            var list = JsonConvert.DeserializeObject<List<OrganizationDivisionMap>>(rm.RequestObj.ToString());
            var response = await _organizationAdministrativeUnitMapService.SaveOrganizationDivisionMap(list);
            return Ok(response);
        }
        [HttpPost]
        [Route("saveOrganizationDistrictMap")]
        public async Task<IActionResult> SaveOrganizationDistrictMap(RequestMessage rm)
        {
            var list = JsonConvert.DeserializeObject<List<OrganizationDistrictMap>>(rm.RequestObj.ToString());
            var response = await _organizationAdministrativeUnitMapService.SaveOrganizationDistrictMap(list);
            return Ok(response);
        }
        [HttpPost]
        [Route("saveOrganizationUpazilaCityCorporationMap")]
        public async Task<IActionResult> SaveOrganizationUpazilaCityCorporationMap(RequestMessage rm)
        {
            var list = JsonConvert.DeserializeObject<List<OrganizationUpazilaCityCorporationMap>>(rm.RequestObj.ToString());
            var response = await _organizationAdministrativeUnitMapService.SaveOrganizationUpazilaCityCorporationMap(list);
            return Ok(response);
        }
        [HttpPost]
        [Route("saveOrganizationThanaMap")]
        public async Task<IActionResult> SaveOrganizationThanaMap(RequestMessage rm)
        {
            var list = JsonConvert.DeserializeObject<List<OrganizationThanaMap>>(rm.RequestObj.ToString());
            var response = await _organizationAdministrativeUnitMapService.SaveOrganizationThanaMap(list);
            return Ok(response);
        }
        [HttpPost]
        [Route("saveOrganizationUnionWardMap")]
        public async Task<IActionResult> SaveOrganizationUnionWardMap(RequestMessage rm)
        {
            var list = JsonConvert.DeserializeObject<List<OrganizationUnionWardMap>>(rm.RequestObj.ToString());
            var response = await _organizationAdministrativeUnitMapService.SaveOrganizationUnionWardMap(list);
            return Ok(response);
        }
        
        [HttpPost]
        [Route("saveOrganizationVillageAreaMap")]
        public async Task<IActionResult> SaveOrganizationVillageAreaMap(RequestMessage rm)
        {
            var list = JsonConvert.DeserializeObject<List<OrganizationVillageAreaMap>>(rm.RequestObj.ToString());
            var response = await _organizationAdministrativeUnitMapService.SaveOrganizationVillageAreaMap(list);
            return Ok(response);
        }
        [HttpPost]
        [Route("saveOrganizationParaMap")]
        public async Task<IActionResult> SaveOrganizationParaMap(RequestMessage rm)
        {
            var list = JsonConvert.DeserializeObject<List<OrganizationParaMap>>(rm.RequestObj.ToString());
            var response = await _organizationAdministrativeUnitMapService.SaveOrganizationParaMap(list);
            return Ok(response);
        }
    }   
}
