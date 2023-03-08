using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.Entities;
using DotNet.ApplicationCore.Utils;
using DotNet.Services.Services.Common;
using DotNet.Services.Services.Infrastructure;
using DotNet.Services.Services.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.WebApi.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ITokenService tokenService;
        private readonly AppSettingsJson appSettingsJson;
        public AuthenticateController(
            ITokenService tokenService, 
            IUserService userService,
            IPermissionService permissionService,
            IGlobalSettingService globalSettingService,
            IOptionsSnapshot<AppSettingsJson> optionsSnapshot
            )
        {
            this.tokenService = tokenService;
            _userService = userService;
            _permissionService = permissionService;
            _globalSettingService = globalSettingService;
            appSettingsJson = optionsSnapshot.Value;
        }
        [HttpPost("authenticate"), AllowAnonymous]
        public async Task<IActionResult> Authenticate(AuthUser user)
        {
            ResponseMessage resMes = _userService.UserAuthentication(user);
            
            AuthUser authUser = (AuthUser)resMes.ResponseObj;
            if (authUser== null || authUser.UserAutoID == 0)
            {
                resMes.StatusCode = ReturnStatus.Failed;
                return await Task.FromResult(Ok(resMes));
            }

            authUser.TokenResult = tokenService.BuildToken(authUser);
            var lstPermission = await _permissionService.GetAll();
            lstPermission = _permissionService.MakeListWithChild((List<Permission>)lstPermission);
            authUser.Permissions = (List<PermissionDTO>)lstPermission;
            authUser.GlobalSettings = await _globalSettingService.GetAllWithUserOrganization();
            resMes.ResponseObj = authUser;
            resMes.StatusCode = ReturnStatus.Success;
            return await Task.FromResult(Ok(resMes));
        }
    }
}
