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
    public class TrainingFormController : ApiControllerBase
    {
        private readonly ITrainingFormService _trainingFormService;

        public TrainingFormController(ITrainingFormService traingingFormService)
        {
            _trainingFormService = traingingFormService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Pagination<TrainingFormViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int currentPage = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sort = "",
            [FromQuery] string queryString = "{ }")
        {
            return await ExecuteFunction(async (user) =>
            {
                var filterObject = JsonSerializer.Deserialize<TrainingFormQueryModel>(queryString);
                filterObject.CurrentPage = currentPage;
                filterObject.PageSize = pageSize;
                filterObject.Sort = sort;

                return await _trainingFormService.GetAllAsync(filterObject);
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TrainingFormViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return await ExecuteFunction(async (user) =>
            {
                var result = await _trainingFormService.GetById(id);
                return result;
            });
        }

        //[HttpPost("")]
        //[ProducesResponseType(typeof(TrainingFormViewModel), StatusCodes.Status200OK)]
        //public async Task<IActionResult> Create(TrainingFormRequestModel requestModel)
        //{
        //    return await ExecuteFunction(async () =>
        //    {
        //        var model = AutoMapperUtils.AutoMap<TrainingFormRequestModel, TrainingForm>(requestModel);
        //        return await _trainingFormService.SaveAsync(model, requestModel.TrainingSubjects);
        //    });
        //}

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TrainingFormViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] TrainingFormRequestModel requestModel)
        {
            return await ExecuteFunction(async (user) =>
            {
                var viewModel = await _trainingFormService.GetById(id);
                var model = AutoMapperUtils.AutoMap<TrainingFormViewModel, TrainingForm>(viewModel);

                if (model == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                model = AutoMapperUtils.AutoMap<TrainingFormRequestModel, TrainingForm>(requestModel, model);
                return await _trainingFormService.SaveAsync(model, requestModel.TrainingSubjects);
            });
        }

        //[HttpPost("delete/many")]
        //[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        //public async Task<IActionResult> DeleteMany([FromBody] Guid[] deleteIds)
        //{
        //    return await ExecuteFunction(async () =>
        //    {
        //        return await _trainingFormService.DeleteManyAsync(deleteIds);
        //    });
        //}
    }
}
