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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _userService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var user = JsonConvert.DeserializeObject<Users>(rm.RequestObj.ToString());
            var response = await _userService.Add(user);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var user = JsonConvert.DeserializeObject<Users>(rm.RequestObj.ToString());
            var response = await _userService.Update(user);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.Delete(id);
            return Ok(response);
        }

        [HttpGet("getAllByOrganizationID")]
        public async Task<IActionResult> GetAllByOrganizationID()
        {
            var response = await _userService.GetAllByOrganizationID();
            return Ok(response);
        }

        [HttpGet]
        [Route("getInitialData")]
        public async Task<IActionResult> GetInitialData()
        {
            var response = await _userService.GetInitialData();
            return Ok(response);
        }
    }
}
