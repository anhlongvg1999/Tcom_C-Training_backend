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
    public class TitleController : ApiControllerBase
    {
        private readonly ITitleService _titleService;

        public TitleController(ITitleService titleService)
        {
            _titleService = titleService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Pagination<TitleViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int currentPage = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sort = "",
            [FromQuery] string queryString = "{ }")
        {
            return await ExecuteFunction(async (user) =>
            {
                var filterObject = JsonSerializer.Deserialize<TitleQueryModel>(queryString);
                filterObject.CurrentPage = currentPage;
                filterObject.PageSize = pageSize;
                filterObject.Sort = sort;

                return await _titleService.GetAllAsync(filterObject);
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TitleViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return await ExecuteFunction(async (user) =>
            {
                var result = await _titleService.GetById(id);
                return AutoMapperUtils.AutoMap<Title, TitleViewModel>(result); ;
            });
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(TitleViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(TitleRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                var model = AutoMapperUtils.AutoMap<TitleRequestModel, Title>(requestModel);
                return await _titleService.SaveAsync(model);
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TitleViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] TitleRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                var model = await _titleService.GetById(id);

                if (model == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                model = AutoMapperUtils.AutoMap<TitleRequestModel, Title>(requestModel, model);
                return await _titleService.SaveAsync(model);
            });
        }

        [HttpPost("delete/many")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMany([FromBody] Guid[] deleteIds)
        {
            return await ExecuteFunction(async (user) =>
            {
                return await _titleService.DeleteManyAsync(deleteIds);
            });
        }
    }
}
