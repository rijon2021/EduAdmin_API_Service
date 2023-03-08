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
    public class ParaController : ControllerBase
    {
        private readonly IParaService _paraService;
        private readonly ILogger<ParaController> _logger;

        public ParaController(IParaService paraService, ILogger<ParaController> logger)
        {
            _paraService = paraService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _paraService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _paraService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var para = JsonConvert.DeserializeObject<VMPara>(rm.RequestObj.ToString());
            var response = await _paraService.Add(para);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var para = JsonConvert.DeserializeObject<VMPara>(rm.RequestObj.ToString());
            var response = await _paraService.Update(para);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _paraService.Delete(id);
            return Ok(response);
        }
        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(RequestMessage rm)
        {
            var userLevels = JsonConvert.DeserializeObject<List<VMPara>>(rm.RequestObj.ToString());
            var response = await _paraService.UpdateOrder(userLevels);
            return Ok(response);
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search(RequestMessage rm)
        {
            QueryObject queryObject = JsonConvert.DeserializeObject<QueryObject>(rm.RequestObj.ToString());
            var response = await _paraService.Search(queryObject);
            return Ok(response);
        }
        [HttpPost("getListByVillageArea")]
        public async Task<IActionResult> GetListByUnion(RequestMessage rm)
        {
            List<VMVillageArea> objList = JsonConvert.DeserializeObject<List<VMVillageArea>>(rm.RequestObj.ToString());
            var response = await _paraService.GetListByVillageArea(objList);
            return Ok(response);
        }
        [HttpGet]
        [Route("getListByOrganizationID/{id}")]
        public async Task<IActionResult> GetListByOrganizationID(int id)
        {
            var response = await _paraService.GetListByOrganizationID(id);
            return Ok(response);
        }

    }
}
