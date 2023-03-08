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
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(IPermissionService userService, ILogger<PermissionController> logger)
        {
            _permissionService = userService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _permissionService.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("getByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var response = await _permissionService.GetByID(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestMessage rm)
        {
            var permission = JsonConvert.DeserializeObject<Permission>(rm.RequestObj.ToString());
            var response = await _permissionService.Add(permission);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RequestMessage rm)
        {
            var permission = JsonConvert.DeserializeObject<Permission>(rm.RequestObj.ToString());
            var response = await _permissionService.Update(permission);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _permissionService.Delete(id);
            return Ok(response);
        }
        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(RequestMessage rm)
        {
            var permissions = JsonConvert.DeserializeObject<List<Permission>>(rm.RequestObj.ToString());
            var response = await _permissionService.UpdateOrder(permissions);
            return Ok(response);
        }
    }
}
