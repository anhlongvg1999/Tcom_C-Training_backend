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
    public class DepartmentController : ApiControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Pagination<DepartmentViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid organizationId,
            [FromQuery] int currentPage = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sort = "",
            [FromQuery] string queryString = "{ }")
        {
            return await ExecuteFunction(async (user) =>
            {
                var filterObject = JsonSerializer.Deserialize<DepartmentQueryModel>(queryString);
                filterObject.CurrentPage = currentPage;
                filterObject.PageSize = pageSize;
                filterObject.Sort = sort;
                filterObject.OrganizationId = organizationId;

                return await _departmentService.GetAllAsync(filterObject);
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartmentViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return await ExecuteFunction(async (user) =>
            {
                var result = await _departmentService.GetById(id);
                return AutoMapperUtils.AutoMap<Department, DepartmentViewModel>(result);
            });
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(DepartmentViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(DepartmentRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                var model = AutoMapperUtils.AutoMap<DepartmentRequestModel, Department>(requestModel);
                return await _departmentService.SaveAsync(model);
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DepartmentViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DepartmentRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                var model = await _departmentService.GetById(id);

                if (model == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                model = AutoMapperUtils.AutoMap<DepartmentRequestModel, Department>(requestModel, model);
                return await _departmentService.SaveAsync(model);
            });
        }

        [HttpPost("delete/many")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMany([FromBody] Guid[] deleteIds)
        {
            return await ExecuteFunction(async (user) =>
            {
                return await _departmentService.DeleteManyAsync(deleteIds);
            });
        }
    }
}
