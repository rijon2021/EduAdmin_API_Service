using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.Entities;
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
    public class GlobalSettingController : ControllerBase
    {
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ILogger<GlobalSettingController> _logger;

        public GlobalSettingController(IGlobalSettingService userService, ILogger<GlobalSettingController> logger)
        {
            _globalSettingService = userService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _globalSettingService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _globalSettingService.GetByID(id);
            return Ok(response);
        }


        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var GlobalSetting = JsonConvert.DeserializeObject<GlobalSetting>(rm.RequestObj.ToString());
            var response = await _globalSettingService.Update(GlobalSetting);
            return Ok(response);
        }
    }
}
