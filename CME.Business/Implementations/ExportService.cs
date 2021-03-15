using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.EntityFrameworkCore;
using SERP.Filenet.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsoft.Framework.Common;
using TSoft.Framework.DB;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ClosedXML.Excel;
using Tsoft.Framework.Office;

namespace CME.Business.Implementations
{
    public class ExportService : IExportService
    {
        private const string CachePrefix = "export-";
        private readonly DataContext _dataContext;
        //private readonly ICacheService _cacheService;
        private readonly IHostingEnvironment _environment;
        //TODO: CACHE
        public ExportService(DataContext dataContext, IHostingEnvironment environment)
        {
            _dataContext = dataContext;
            _environment = environment;
        }

        public async Task<MemoryStream> ReportByDepartment(int fromYear, int toYear)
        {
            IXLWorkbook workbook = new XLWorkbook();
            IXLWorksheet overviewSheet;
            var templatePath = Path.Combine(_environment.WebRootPath, "templates", "report-by-department-2021-03-08.xlsx");

            using (var templatelWorkbook = new XLWorkbook(templatePath))
            {
                if (templatelWorkbook.Worksheets.TryGetWorksheet("Sheet1", out overviewSheet))
                {
                    var departments = await _dataContext.Departments.OrderBy(x => x.Name).ToListAsync();
                    var listTrainingProgram_User = new List<List<TrainingProgram_User>>();
                    var listYear = new List<int>();
                    var firstRowOfDepartment = 3;
                    var firstRowOfYear = 2;
                    var firstColOfYear = 4;
                    var lastRowOfDepartment = departments.Count + firstRowOfDepartment;

                    for (var year = fromYear; year <= toYear; year++)
                    {
                        listYear.Add(year);
                        listTrainingProgram_User.Add(await _dataContext.TrainingProgram_Users.Where(x => x.Year == year).Include(x => x.User).ToListAsync());
                    }

                    // In năm xét
                    for (var i = 0; i < listYear.Count; i++)
                    {
                        overviewSheet.Cell(firstRowOfYear, firstColOfYear + i).Value = listYear[i];
                    }

                    // Merge cột năm xét
                    overviewSheet.Range(overviewSheet.Cell(1, firstColOfYear), overviewSheet.Cell(1, firstColOfYear + listYear.Count - 1)).Merge();


                    for (var i = 0; i < departments.Count; i++)
                    {
                        //STT
                        var STTCell = overviewSheet.Cell(firstRowOfDepartment + i, 1);
                        STTCell.Value = i + 1;
                        STTCell.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        STTCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        // Tên khoa phòng
                        var departmentCell = overviewSheet.Cell(firstRowOfDepartment + i, 2);
                        departmentCell.Value = departments[i].Name;
                        departmentCell.Style.Font.SetBold(true);
                        departmentCell.Style.Alignment.SetWrapText(true);

                        // Số nhân viên trong khoa phòng
                        var numOfUsers = _dataContext.Users.Where(x => x.DepartmentId == departments[i].Id).Count();
                        overviewSheet.Cell(firstRowOfDepartment + i, 3).Value = numOfUsers;

                        // Số nhân viên đủ điều kiện
                        for (var y = 0; y < listYear.Count; y++)
                        {
                            // Số người đủ điều kiện
                            var eligibleUsers = listTrainingProgram_User[y]
                                .Where(u => u.User.DepartmentId == departments[i].Id)
                                .GroupBy(u => u.UserId)
                                .Select(grp => new
                                {
                                    Amount = grp.Sum(x => x.Amount)
                                })
                                .Where(x => x.Amount >= 24)
                                .Count();

                            overviewSheet.Cell(firstRowOfDepartment + i, firstColOfYear + y).Value = eligibleUsers;
                        }
                    }

                    var totalUserCell = overviewSheet.Cell(lastRowOfDepartment + 1, 3);
                    totalUserCell.FormulaA1 = OfficeUtils.Sum("C" + firstRowOfDepartment, "C" + lastRowOfDepartment);

                    // Border
                    var borderRange = OfficeUtils.GetRange(1, overviewSheet.ColumnsUsed().Count(), 1, overviewSheet.RowsUsed().Count());
                    overviewSheet.Range(borderRange).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    workbook.AddWorksheet(overviewSheet);
                }

            }

            MemoryStream memorystream = new MemoryStream();
            workbook.SaveAs(memorystream);
            return memorystream;
        }

        private void InvalidCache(Guid id)
        {
            //string cacheKey = BuildCacheKey(id);
            //string cacheMasterKey = BuildCacheMasterKey();

            //_cacheService.Remove(cacheKey);
            //_cacheService.Remove(cacheMasterKey);
        }
        private string BuildCacheKey(Guid id)
        {
            return $"{CachePrefix}{id}";
        }

        private string BuildCacheMasterKey()
        {
            return $"{CachePrefix}*";
        }

    }
}
