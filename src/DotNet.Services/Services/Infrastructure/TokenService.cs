
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using DotNet.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Http;

namespace DotNet.Services.Services.Infrastructure
{
    public class TokenService : ITokenService
    {
        private TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);
        public IConfiguration _configuration;
        public TokenService(IConfiguration config)
        {
            _configuration = config;
        }
        public TokenResult BuildToken(AuthUser user)
        {
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserID),
                        new Claim("UserAutoID", user.UserAutoID.ToString()),
                        new Claim("UserTypeID", user.UserTypeID.ToString()),
                        new Claim("OrganizationID", user.OrganizationID.ToString()),
                        new Claim("DesignationID", user.DesignationID.ToString()),
                        new Claim("UserFullName", user.UserFullName),
                        new Claim("UserRoleID", user.UserRoleID.ToString()),
                    };

            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            //var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            //    expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn);
            var tokenResult = new TokenResult
            {
                Access_token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            };
            return tokenResult;
        }




        //private async Task<TokenResult> BuildToken(JwtTokenInfo request)
        //{
        //    TokenResult tokenResult = new TokenResult();
        //    try
        //    {
        //        var info = await _storedProcedureService.GetTokenInfo(request.Password, request.Username);
        //        if (info.StatusCode == 200)
        //        {
        //            string requestOrigin = HttpContext.Request.Headers["origin"].ToString();
        //            //Master User restriction
        //            if (_appSettingsJson.MasterLoginOrigin?.Split(";")?.Any(x => x == requestOrigin) == true)
        //            {
        //                if (info.UserTypeId == (int)EnumUserType.Master)
        //                {
        //                    //ip restriction
        //                    if (!await _masterCompanyService.CheckMasterLoginIPAccess(info.CompanyId, info.UserId, request.Ip))
        //                    {
        //                        tokenResult.StatusCode = StatusCodes.Status401Unauthorized;
        //                        tokenResult.Message = "Your IP address is restricted!";
        //                        return await Task.FromResult(tokenResult);
        //                    }
        //                }
        //                else
        //                {
        //                    tokenResult.StatusCode = StatusCodes.Status401Unauthorized;
        //                    tokenResult.Message = "Unauthorized! Master user only.";
        //                    return await Task.FromResult(tokenResult);
        //                }
        //            }
        //            //Client User restriction
        //            else if (info.UserTypeId == (int)EnumUserType.Client)
        //            {
        //                if (_appSettingsJson.ClientLoginOrigin?.Split(";")?.Any(x => x == requestOrigin) == true)
        //                {
        //                }
        //                else
        //                {
        //                    tokenResult.StatusCode = StatusCodes.Status401Unauthorized;
        //                    tokenResult.Message = "Unauthorized! No access to Client User.";
        //                    return await Task.FromResult(tokenResult);
        //                }
        //            }

        //            string solrAfHeader = HttpContext.Request.Headers["SyncFunctionCall"].ToString();

        //            info.CompanyGuid ??= "";
        //            var _rights = string.Join(",", _roleRoghtService.Queryable().Where(x => x.CompanyId == info.CompanyId && x.RoleId == info.RoleId).Select(x => x.RightId));
        //            //create token
        //            var token = await GenerateToken(request.Username, info.CompanyName, info.UserName, info.UserFullName, info.UserId.ToString(), info.CompanyId.ToString(), info.CompanyGuid.ToString(), info.StorageName, _rights, info.TimeZoneId, info.DateTimeFormat, info.RoleId, info.LanguageCountryId, info.BranchId, info.BranchGuid, false, solrAfHeader);
        //            if (request.Grant_type?.ToLower() == "demo" || request.Grant_type?.ToLower() == "demouk")
        //            {
        //                tokenResult.UserEmail = request.Username;
        //            }
        //            tokenResult.Access_token = new JwtSecurityTokenHandler().WriteToken(token);
        //            tokenResult.Expiration = token.ValidTo;
        //            tokenResult.StatusCode = StatusCodes.Status200OK;
        //            tokenResult.Message = "Success";
        //            return await Task.FromResult(tokenResult);
        //        }
        //        tokenResult.StatusCode = info.StatusCode;
        //        tokenResult.Message = info.Message;
        //        return await Task.FromResult(tokenResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private async Task<JwtSecurityToken> GenerateToken(string email, string companyName, string userName, string userFullName, string userId, string companyId, string companyGuid, string storageName, string rightids = "", string timezoneId = "", string dateTimeFormat = "", int RoleId = 0, int languageCountryId = 239, int? branchId = 0, int? branchGuid = 0, bool IsRedisCacheEnable = false, string solrAfHeader = "")
        //{
        //    string sessionid = LiteDB.ObjectId.NewObjectId().ToString();
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettingsJson.Jwt_Key));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    IEnumerable<Claim> claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, email, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(JwtRegisteredClaimNames.UniqueName, email, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.UserId.ToString(), userId, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.UserName.ToString(), userName, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.CompanyName.ToString(), companyName, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.CompanyId.ToString(), companyId, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.Email.ToString(), email, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.StorageName.ToString(), storageName, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        //new Claim(EnumClaimType.PSK.ToString(), "ANQH4P3WD3BBI5KE", JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.Rights.ToString(), rightids, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.TimeZoneId.ToString(), timezoneId, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.SessionId.ToString(),sessionid , JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.DateTimeFormat.ToString(), dateTimeFormat, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.UserFullName.ToString(), userFullName, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.CompanyGuid.ToString(), companyGuid, JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.RoleId.ToString(), RoleId.ToString(), JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.LanguageCountryId.ToString(), languageCountryId.ToString(), JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.BranchId.ToString(), branchId?.ToString()?? "0", JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        new Claim(EnumClaimType.IsRedisCacheEnable.ToString(), IsRedisCacheEnable.ToString()?? "0", JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //        //new Claim(EnumClaimType.BranchGuid.ToString(), branchGuid?.ToString()?? "0", JwtSecurityTokenHandler.JsonClaimTypeProperty),
        //    };
        //    var token = new JwtSecurityToken(_appSettingsJson.Service_base, _appSettingsJson.Origins, claims, expires: solrAfHeader != "" && solrAfHeader.Equals("solrcontext") ? DateTime.UtcNow.AddYears(10) : DateTime.UtcNow.AddHours(8), signingCredentials: credentials);
        //    if (rightids.Any())
        //    {
        //        //await CreateSession(rightids.Split(",", StringSplitOptions.None).ToList(), email, string.Empty, sessionid);
        //    }
        //    return await Task.FromResult(token);
        //}

        //private async Task<ActionResult> BuildRefreshToken(string token)
        //{
        //    try
        //    {
        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettingsJson.Jwt_Key));
        //        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //        var handler = new JwtSecurityTokenHandler();
        //        var refreshToken = handler.ReadToken(token) as JwtSecurityToken;
        //        var claims = refreshToken.Claims.ToList();
        //        //Remove Aud(audience) from previous token
        //        claims?.RemoveAll(x => x.Type == JwtRegisteredClaimNames.Aud);
        //        var newToken = new JwtSecurityToken(_appSettingsJson.Service_base, _appSettingsJson.Origins, claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: credentials);
        //        return await Task.FromResult(Ok(new
        //        {
        //            access_token = new JwtSecurityTokenHandler().WriteToken(newToken),
        //            expiration = newToken.ValidTo
        //        }));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
