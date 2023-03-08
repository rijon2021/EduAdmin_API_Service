using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.Utils;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DotNet.Services.Services.Infrastructure;
using DotNet.Services.Services.Interfaces.Common;

namespace DotNet.WebApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Authorization")]

    
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService tokenService;
        private readonly AppSettingsJson appSettingsJson;
        //private readonly HttpContext http;

        public AuthorizationController( ITokenService tokenService, IUserService userService, IOptionsSnapshot<AppSettingsJson> optionsSnapshot)
        {
            this.tokenService= tokenService;
            this._userService = userService;
           // this.http = http;
            this.appSettingsJson = optionsSnapshot.Value;
        }

        //[HttpPost("token") ,AllowAnonymous]
        //public async Task<IActionResult> Token(UserResponse user)
        //{
        //    //{
        //    //    "requestObject":{ "UserName":"daud","Password":"123456"}
        //    //}
        //    //UserModel userModel = new UserModel();
        //    //var userModel = JsonConvert.DeserializeObject<UserModel>(requestObj.requestObject.ToString());
        //    //if (request.Grant_type == "password")
        //    //{
        //    //    var response = await BuildToken(request);
        //    //    return await Task.FromResult(Ok(response));
        //    //}
        //    //else if (request.Grant_type == "withoutPassword")
        //    //{
        //    //    var response = await BuildTokenForAnotherCompany(Convert.ToInt16(request.CompanyId), request.Username);
        //    //    return await Task.FromResult(Ok(response));
        //    //}
        //    //else if (request.Grant_type == "refresh_token")
        //    //{
        //    //    var response = await BuildRefreshToken(request.Refreshtoken);
        //    //    return response;
        //    //}
        //    // var userModel = await HttpContext.Request.ReadFromJsonAsync<UserModel>();
          
        //    var userDto = _userService.GetUser(user);
        //    if (userDto == null)
        //    {
        //        HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //        return await Task.FromResult(Ok(HttpContext.Response.StatusCode));
        //    }          
        //    var token = tokenService.BuildToken(userDto);
        //    //await HttpContext.Response.WriteAsJsonAsync(new { token = token });
        //  //  return;
        //    return await Task.FromResult(Ok(new { token }));
        //}
       
        ////GET : /logout
        //[HttpGet("~/logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    try
        //    {
        //        var principal = HttpContext.User;
        //        if (principal == null) throw new Exception("User's Compnay Cannot found");
        //        var claims = principal.Claims.ToList();
        //        var email = claims.FirstOrDefault(c => c.Type == "Email")?.Value;
        //        return await Task.FromResult(Ok("Logout Successful"));
        //    }
        //    catch (Exception ex)
        //    {
        //        await _errorService.SaveError(ClassName, "ReExchange", ex.ToString(), _unitOfWork);
        //        return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
        //    }
        //}
        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);

        //        var authClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        };

        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        }

        //        var token = CreateToken(authClaims);
        //        var refreshToken = GenerateRefreshToken();

        //        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        //        user.RefreshToken = refreshToken;
        //        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        //        await _userManager.UpdateAsync(user);

        //        return Ok(new
        //        {
        //            Token = new JwtSecurityTokenHandler().WriteToken(token),
        //            RefreshToken = refreshToken,
        //            Expiration = token.ValidTo
        //        });
        //    }
        //    return Unauthorized();
        //}
    }
}
