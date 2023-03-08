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
    public class VillageAreaController : ControllerBase
    {
        private readonly IVillageAreaService _villageAreaService;
        private readonly ILogger<VillageAreaController> _logger;

        public VillageAreaController(IVillageAreaService villageAreaService, ILogger<VillageAreaController> logger)
        {
            _villageAreaService = villageAreaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _villageAreaService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _villageAreaService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var villageArea = JsonConvert.DeserializeObject<VMVillageArea>(rm.RequestObj.ToString());
            var response = await _villageAreaService.Add(villageArea);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var villageArea = JsonConvert.DeserializeObject<VMVillageArea>(rm.RequestObj.ToString());
            var response = await _villageAreaService.Update(villageArea);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _villageAreaService.Delete(id);
            return Ok(response);
        }
        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(RequestMessage rm)
        {
            var userLevels = JsonConvert.DeserializeObject<List<VMVillageArea>>(rm.RequestObj.ToString());
            var response = await _villageAreaService.UpdateOrder(userLevels);
            return Ok(response);
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search(RequestMessage rm)
        {
            QueryObject queryObject = JsonConvert.DeserializeObject<QueryObject>(rm.RequestObj.ToString());
            var response = await _villageAreaService.Search(queryObject);
            return Ok(response);
        }
        [HttpPost("getListByUnionWard")]
        public async Task<IActionResult> GetListByUnionWard(RequestMessage rm)
        {
            List<VMUnionWard> objList = JsonConvert.DeserializeObject<List<VMUnionWard>>(rm.RequestObj.ToString());
            var response = await _villageAreaService.GetListByUnionWard(objList);
            return Ok(response);
        }
        [HttpGet]
        [Route("getListByOrganizationID/{id}")]
        public async Task<IActionResult> GetListByOrganizationID(int id)
        {
            var response = await _villageAreaService.GetListByOrganizationID(id);
            return Ok(response);
        }


    }
}
