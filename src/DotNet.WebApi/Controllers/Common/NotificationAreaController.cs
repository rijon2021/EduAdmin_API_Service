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
    public class NotificationAreaController : ControllerBase
    {
        private readonly INotificationAreaService _userLevelService;
        private readonly ILogger<NotificationAreaController> _logger;

        public NotificationAreaController(INotificationAreaService userService, ILogger<NotificationAreaController> logger)
        {
            _userLevelService = userService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userLevelService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _userLevelService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var NotificationArea = JsonConvert.DeserializeObject<NotificationArea>(rm.RequestObj.ToString());
            var response = await _userLevelService.Add(NotificationArea);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var NotificationArea = JsonConvert.DeserializeObject<NotificationArea>(rm.RequestObj.ToString());
            var response = await _userLevelService.Update(NotificationArea);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userLevelService.Delete(id);
            return Ok(response);
        }
    }
}
