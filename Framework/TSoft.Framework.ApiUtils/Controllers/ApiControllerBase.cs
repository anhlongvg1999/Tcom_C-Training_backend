using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tsoft.Framework.Common.Configs;

namespace TSoft.Framework.ApiUtils.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected async Task<IActionResult> ExecuteFunction<T>(string permission, Func<string, Task<T>> func)
        {
            var result = await func(permission);
            return new OkObjectResult(new GenericResult((object)result, true, "success!!!"));
        }
        protected async Task<IActionResult> ExecuteFunction<T>(Func<RequestUser, Task<T>> func)
        {
            try
            {
                var requestUser = new RequestUser();
                var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);

                //var tonkenPrefix = _configuration.GetSection(nameof(JWTAuthenticaion)).Get<JWTAuthenticaion>();
                string secret = "372F78BC6B66F3CEAF705FE57A91F369A5BE956692A4DA7DE16CAD71113CF046";
                var key = Encoding.ASCII.GetBytes(secret);
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var claims = new ClaimsPrincipal();
                try
                {
                    claims = handler.ValidateToken(token, validations, out var tokenSecure);
                }
                catch (Exception e)
                {
                    return new UnauthorizedResult();
                }

                Guid userId = Guid.Empty;
                if (Guid.TryParse(claims.Claims.First(claim => claim.Type == "Id").Value, out userId))
                {
                    requestUser.Id = userId;
                }
                requestUser.Fullname = claims.Claims.First(claim => claim.Type == "Fullname").Value;

                var roles = claims.Claims.First(claim => claim.Type == "Roles").Value;
                if (!string.IsNullOrEmpty(roles))
                {
                    var listRoles = roles.Split(",").ToList();
                    requestUser.Roles = listRoles;
                }


                var result = await func(requestUser);
                return new OkObjectResult(new GenericResult(result, true, "success!!!"));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }

        protected async Task<IActionResult> ExecuteFunction<T>(Func<Task<T>> func)
        {
            try
            {
                var result = await func();
                return new OkObjectResult(new GenericResult(result, true, "success!!!"));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }
    }
}
