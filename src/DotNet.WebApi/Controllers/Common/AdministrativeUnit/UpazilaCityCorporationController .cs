using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Services.Common;
using DotNet.Services.Services.Common.AdministrativeUnit;
using DotNet.Services.Services.Interfaces.Common.AdministrativeUnit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Description;
using static AutoMapper.Internal.ExpressionFactory;

namespace DotNet.WebApi.Controllers.Common.AdministrativeUnit
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UpazilaCityCorporationController : ControllerBase
    {
        private readonly IUpazilaCityCorporationService _upazilaCityCorporationService;
        private readonly ILogger<UpazilaCityCorporationController> _logger;

        public UpazilaCityCorporationController(IUpazilaCityCorporationService upazilaCityCorporationService, ILogger<UpazilaCityCorporationController> logger)
        {
            _upazilaCityCorporationService = upazilaCityCorporationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _upazilaCityCorporationService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _upazilaCityCorporationService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var upazila = JsonConvert.DeserializeObject<VMUpazilaCityCorporation>(rm.RequestObj.ToString());
            var response = await _upazilaCityCorporationService.Add(upazila);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var upazila = JsonConvert.DeserializeObject<VMUpazilaCityCorporation>(rm.RequestObj.ToString());
            var response = await _upazilaCityCorporationService.Update(upazila);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _upazilaCityCorporationService.Delete(id);
            return Ok(response);
        }
        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(RequestMessage rm)
        {
            var userLevels = JsonConvert.DeserializeObject<List<VMUpazilaCityCorporation>>(rm.RequestObj.ToString());
            var response = await _upazilaCityCorporationService.UpdateOrder(userLevels);
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(RequestMessage rm)
        {
            QueryObject queryObject = JsonConvert.DeserializeObject<QueryObject>(rm.RequestObj.ToString());
            var response = await _upazilaCityCorporationService.Search(queryObject);
            return Ok(response);
        }
        [HttpPost("getListByDistrict")]
        public async Task<IActionResult> GetListByDistrict(RequestMessage rm)
        {
            List<VMDistrict> objList = JsonConvert.DeserializeObject<List<VMDistrict>>(rm.RequestObj.ToString());
            var response = await _upazilaCityCorporationService.GetListByDistrict(objList);
            return Ok(response);
        }
        [HttpGet]
        [Route("getListByOrganizationID/{id}")]
        public async Task<IActionResult> GetListByOrganizationID(int id)
        {
            var response = await _upazilaCityCorporationService.GetListByOrganizationID(id);
            return Ok(response);
        }


    }
}
