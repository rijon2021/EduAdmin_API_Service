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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _countryService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _countryService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var country = JsonConvert.DeserializeObject<VMCountry>(rm.RequestObj.ToString());
            var response = await _countryService.Add(country);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var country = JsonConvert.DeserializeObject<VMCountry>(rm.RequestObj.ToString());
            var response = await _countryService.Update(country);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _countryService.Delete(id);
            return Ok(response);
        }

        [HttpPost("getListByOrganization")]
        public async Task<IActionResult> GetListByOrganization(RequestMessage rm)
        {
            List<Organization> objList = JsonConvert.DeserializeObject<List<Organization>>(rm.RequestObj.ToString());
            var response = await _countryService.GetListByOrganization(objList);
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(RequestMessage rm)
        {
            QueryObject queryObject = JsonConvert.DeserializeObject<QueryObject>(rm.RequestObj.ToString());
            var response = await _countryService.Search(queryObject);
            return Ok(response);
        }
    }
}
