using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Services.Common;
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
    public class DivisionController : ControllerBase
    {
        private readonly IDivisionService _divisionService;
        private readonly ILogger<DivisionController> _logger;

        public DivisionController(IDivisionService divisionService, ILogger<DivisionController> logger)
        {
            _divisionService = divisionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _divisionService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _divisionService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var division = JsonConvert.DeserializeObject<VMDivision>(rm.RequestObj.ToString());
            var response = await _divisionService.Add(division);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var division = JsonConvert.DeserializeObject<VMDivision>(rm.RequestObj.ToString());
            var response = await _divisionService.Update(division);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _divisionService.Delete(id);
            return Ok(response);
        }
        //[HttpGet]
        //[Route("getListByOrganization/{id}")]
        //public async Task<IActionResult> GetListByOrganization(int id)
        //{
        //    var response = await _divisionService.GetListByOrganization(id);
        //    return Ok(response);
        //}
        [HttpPost("getListByCountry")]
        public async Task<IActionResult> GetListByCountry(RequestMessage rm)
        {
            List<VMCountry> objList = JsonConvert.DeserializeObject<List<VMCountry>>(rm.RequestObj.ToString());
            var response = await _divisionService.GetListByCountry(objList);
            return Ok(response);
        }
        [HttpGet]
        [Route("getListByOrganizationID/{id}")]
        public async Task<IActionResult> GetListByOrganizationID(int id)
        {
            var response = await _divisionService.GetListByOrganizationID(id);
            return Ok(response);
        }
    }
}
