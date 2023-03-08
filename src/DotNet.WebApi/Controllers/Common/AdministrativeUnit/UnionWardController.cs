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
    public class UnionWardController : ControllerBase
    {
        private readonly IUnionWardService _unionWardService;
        private readonly ILogger<UnionWardController> _logger;

        public UnionWardController(IUnionWardService unionWardService, ILogger<UnionWardController> logger)
        {
            _unionWardService = unionWardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _unionWardService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _unionWardService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var unionWard = JsonConvert.DeserializeObject<VMUnionWard>(rm.RequestObj.ToString());
            var response = await _unionWardService.Add(unionWard);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var unionWard = JsonConvert.DeserializeObject<VMUnionWard>(rm.RequestObj.ToString());
            var response = await _unionWardService.Update(unionWard);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _unionWardService.Delete(id);
            return Ok(response);
        }
        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(RequestMessage rm)
        {
            var userLevels = JsonConvert.DeserializeObject<List<VMUnionWard>>(rm.RequestObj.ToString());
            var response = await _unionWardService.UpdateOrder(userLevels);
            return Ok(response);
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search(RequestMessage rm)
        {
            QueryObject queryObject = JsonConvert.DeserializeObject<QueryObject>(rm.RequestObj.ToString());
            var response = await _unionWardService.Search(queryObject);
            return Ok(response);
        }
        [HttpPost("getListByThana")]
        public async Task<IActionResult> GetListByThana(RequestMessage rm)
        {
            List<VMThana> objList = JsonConvert.DeserializeObject<List<VMThana>>(rm.RequestObj.ToString());
            var response = await _unionWardService.GetListByThana(objList);
            return Ok(response);
        }
        [HttpGet]
        [Route("getListByOrganizationID/{id}")]
        public async Task<IActionResult> GetListByOrganizationID(int id)
        {
            var response = await _unionWardService.GetListByOrganizationID(id);
            return Ok(response);
        }

    }
}
