using ClosedXML.Excel;
using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using CME.Entities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERP.Filenet.DB;
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
    public class ExportController : ApiControllerBase
    {
        private DataContext _dataContext;
        private IExportService _exportService;
        public ExportController(DataContext dataContext, IExportService exportService)
        {
            _dataContext = dataContext;
            _exportService = exportService;
        }

        [HttpPost("report-by-department")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReportByDepartment([FromBody] ExportReportByDepartmentRequestModel requestModel)
        {
            var memoryStream = await _exportService.ReportByDepartment(requestModel.FromYear, requestModel.ToYear);
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Download.xlsx");
        }

    }
}
