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
    public class OrganizationController : ApiControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Pagination<OrganizationViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int currentPage = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sort = "",
            [FromQuery] string queryString = "{ }")
        {
            return await ExecuteFunction(async (user) =>
            {
                var filterObject = JsonSerializer.Deserialize<OrganizationQueryModel>(queryString);
                filterObject.CurrentPage = currentPage;
                filterObject.PageSize = pageSize;
                filterObject.Sort = sort;

                return await _organizationService.GetAllAsync(filterObject);
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrganizationViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return await ExecuteFunction(async (user) =>
            {
                var result = await _organizationService.GetById(id);
                return AutoMapperUtils.AutoMap<Organization, OrganizationViewModel>(result); ;
            });
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(OrganizationViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(OrganizationRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                var model = AutoMapperUtils.AutoMap<OrganizationRequestModel, Organization>(requestModel);
                return await _organizationService.SaveAsync(model);
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrganizationRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                var model = await _organizationService.GetById(id);

                if (model == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                model = AutoMapperUtils.AutoMap<OrganizationRequestModel, Organization>(requestModel, model);
                return await _organizationService.SaveAsync(model);
            });
        }

        [HttpPost("delete/many")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMany([FromBody] Guid[] deleteIds)
        {
            return await ExecuteFunction(async (user) =>
            {
                return await _organizationService.DeleteManyAsync(deleteIds);
            });
        }
    }
}
