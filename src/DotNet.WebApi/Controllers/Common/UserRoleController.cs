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
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(IUserRoleService userRoleService, ILogger<UserRoleController> logger)
        {
            _userRoleService = userRoleService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userRoleService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _userRoleService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var UserLevel = JsonConvert.DeserializeObject<UserRole>(rm.RequestObj.ToString());
            var response = await _userRoleService.Add(UserLevel);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var UserLevel = JsonConvert.DeserializeObject<UserRole>(rm.RequestObj.ToString());
            var response = await _userRoleService.Update(UserLevel);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userRoleService.Delete(id);
            return Ok(response);
        }
        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(RequestMessage rm)
        {
            var userLevels = JsonConvert.DeserializeObject<List<UserRole>>(rm.RequestObj.ToString());
            var response = await _userRoleService.UpdateOrder(userLevels);
            return Ok(response);
        }
    }
}
