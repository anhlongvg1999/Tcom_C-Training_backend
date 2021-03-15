using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using CME.Entities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Pagination<UserViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int year,
            [FromQuery] Guid organizationId,
            [FromQuery] Guid departmentId,
            [FromQuery] Guid titleId,
            [FromQuery] int currentPage = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sort = "",
            [FromQuery] string queryString = "{ }")
        {
            return await ExecuteFunction(async (user) =>
            {
                var filterObject = JsonSerializer.Deserialize<UserQueryModel>(queryString);
                filterObject.CurrentPage = currentPage;
                filterObject.PageSize = pageSize;
                filterObject.Sort = sort;
                filterObject.DepartmentId = departmentId;
                filterObject.OrganizationId = organizationId;
                filterObject.TitleId = titleId;
                filterObject.Year = year;

                return await _userService.GetAllAsync(filterObject);
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return await ExecuteFunction(async (user) =>
            {
                var result = await _userService.GetById(id);
                return AutoMapperUtils.AutoMap<User, UserViewModel>(result); ;
            });
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] UserRequestModel requestModel)
        {
            return await ExecuteFunction(async () =>
            {
                var model = AutoMapperUtils.AutoMap<UserRequestModel, User>(requestModel);

                //TODO: FAKE PASSWORD
                var passwordHasher = new PasswordHasher<User>();
                model.Password = passwordHasher.HashPassword(model, Default.Password);


                var result = await _userService.SaveAsync(model, requestModel.AvatarFile);
                return AutoMapperUtils.AutoMap<User, UserViewModel>(result);
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromForm] UserRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                var model = await _userService.GetById(id);

                if (model == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                var newModel = AutoMapperUtils.AutoMap<UserRequestModel, User>(requestModel, model);
                var result = await _userService.SaveAsync(newModel, requestModel.AvatarFile);
                return AutoMapperUtils.AutoMap<User, UserViewModel>(result);
            });
        }

        [HttpPost("delete/many")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMany([FromBody] Guid[] deleteIds)
        {
            return await ExecuteFunction(async (user) =>
            {
                return await _userService.DeleteManyAsync(deleteIds);
            });
        }


        [HttpGet("{id}/training-programs")]
        [ProducesResponseType(typeof(List<TrainingProgram_User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId(Guid id, [FromQuery] int year)
        {
            return await ExecuteFunction(async (user) =>
            {
                var result = await _userService.GetTrainingPrograms(id, year);
                return result;
            });
        }
    }
}
