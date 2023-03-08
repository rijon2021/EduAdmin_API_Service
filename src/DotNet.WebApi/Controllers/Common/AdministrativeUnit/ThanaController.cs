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

namespace DotNet.WebApi.Controllers.Common.AdministrativeUnit
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ThanaController : ControllerBase
    {
        private readonly IThanaService _thanaService;
        private readonly ILogger<ThanaController> _logger;

        public ThanaController(IThanaService districtService, ILogger<ThanaController> logger)
        {
            _thanaService = districtService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _thanaService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _thanaService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var district = JsonConvert.DeserializeObject<VMThana>(rm.RequestObj.ToString());
            var response = await _thanaService.Add(district);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var district = JsonConvert.DeserializeObject<VMThana>(rm.RequestObj.ToString());
            var response = await _thanaService.Update(district);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _thanaService.Delete(id);
            return Ok(response);
        }
        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(RequestMessage rm)
        {
            var userLevels = JsonConvert.DeserializeObject<List<VMThana>>(rm.RequestObj.ToString());
            var response = await _thanaService.UpdateOrder(userLevels);
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(RequestMessage rm)
        {
            QueryObject queryObject = JsonConvert.DeserializeObject<QueryObject>(rm.RequestObj.ToString());
            var response = await _thanaService.Search(queryObject);
            return Ok(response);
        }
        [HttpPost("getListByUpazilaCityCorporation")]
        public async Task<IActionResult> GetListByUpazilaCityCorporation(RequestMessage rm)
        {
            List<VMUpazilaCityCorporation> objList = JsonConvert.DeserializeObject<List<VMUpazilaCityCorporation>>(rm.RequestObj.ToString());
            var response = await _thanaService.GetListByUpazilaCityCorporation(objList);
            return Ok(response);
        }
        [HttpGet]
        [Route("getListByOrganizationID/{id}")]
        public async Task<IActionResult> GetListByOrganizationID(int id)
        {
            var response = await _thanaService.GetListByOrganizationID(id);
            return Ok(response);
        }

    }
}
