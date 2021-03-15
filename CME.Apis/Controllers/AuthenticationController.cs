using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tsoft.Framework.Common;
using TSoft.Framework.ApiUtils.Controllers;
using TSoft.Framework.DB;

namespace CME.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService titleService)
        {
            _authenticationService = titleService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequestModel requestModel)
        {
            return await ExecuteFunction(async () =>
            {
                return await _authenticationService.Login(requestModel.Username, requestModel.Password);
            });
        }

        [HttpPut("change-password")]
        [ProducesResponseType(typeof(LoginResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                if(string.Compare(requestModel.NewPassword, requestModel.ConfirmPassword) != 0)
                {
                    throw new ArgumentException($"Mật khẩu không trùng khớp");
                }
                return await _authenticationService.ChangePassword(user.Id, requestModel.OldPassword, requestModel.NewPassword);
            });
        }
    }
}
