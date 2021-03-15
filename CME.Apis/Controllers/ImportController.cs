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
    public class ImportController : ApiControllerBase
    {
        private DataContext _dataContext;
        public ImportController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("import-user")]
        [ProducesResponseType(typeof(TitleViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportUser([FromForm] ImportUserRequestModel requestModel)
        {
            return await ExecuteFunction(async (requestUser) =>
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Đoc excel
                        var workbook = new XLWorkbook(requestModel.File.OpenReadStream());
                        IXLWorksheet worksheet;

                        if (workbook.Worksheets.TryGetWorksheet(requestModel.SheetName, out worksheet))
                        {
                            var rowUser = new Dictionary<int, User>();
                            var colTrainingProgram = new Dictionary<int, TrainingProgram>();

                            int firstRowOfUser = 5;
                            int rowOfOrg = 4;
                            int rowOfFromDate = 2;
                            int rowOfToDate = 3;
                            int rowOfDTDN = 12;

                            int firstColOfTrp = 13;

                            var lastRow = worksheet.RowsUsed().Count();
                            var lastCol = worksheet.ColumnsUsed().Count();



                            #region Remove old data
                            var oldUsers = _dataContext.Users.ToList();
                            _dataContext.RemoveRange(oldUsers);
                            await _dataContext.SaveChangesAsync();

                            var oldTitles = _dataContext.Titles.ToList();
                            _dataContext.RemoveRange(oldTitles);
                            await _dataContext.SaveChangesAsync();


                            var oldDepartments = _dataContext.Departments.ToList();
                            _dataContext.RemoveRange(oldDepartments);
                            await _dataContext.SaveChangesAsync();

                            var oldTrainingProgram_Users = _dataContext.TrainingProgram_Users.ToList();
                            _dataContext.RemoveRange(oldTrainingProgram_Users);
                            await _dataContext.SaveChangesAsync();

                            var oldTrainingProgram = _dataContext.TrainingPrograms.ToList();
                            _dataContext.RemoveRange(oldTrainingProgram);
                            await _dataContext.SaveChangesAsync();

                            var oldOrganizations = _dataContext.Organizations.Where(x => x.Id != Guid.Parse(Default.OrganizationId)).ToList();
                            _dataContext.RemoveRange(oldOrganizations);
                            await _dataContext.SaveChangesAsync();
                            #endregion

                            #region Add User, Title, Department
                            var newTitles = new List<Title>();
                            var newDepartments = new List<Department>();
                            var newUsers = new List<User>();
                            var listErrorUsername = new List<string>();

                            for (int i = firstRowOfUser; i <= lastRow; i++)
                            {

                                //Họ và tên
                                string fullname = worksheet.Cell(i, 2).Value.ToString().Trim();
                                string certificationNumber = worksheet.Cell(i, 3).Value.ToString().Trim();
                                string issueDateStr = worksheet.Cell(i, 5).Value.ToString();
                                DateTime issueDate;
                                try
                                {
                                    issueDate = DateTime.FromOADate(double.Parse(issueDateStr.ToString()));
                                }
                                catch
                                {
                                    issueDate = new DateTime();
                                }
                                var words = fullname.Trim().Split(' ');

                                var user = new User();
                                user.Id = Guid.NewGuid();
                                user.Fullname = fullname;
                                user.Firstname = words[words.Length - 1];
                                user.CertificateNumber = certificationNumber;

                                if(!string.IsNullOrEmpty(certificationNumber) && certificationNumber != "1")
                                {
                                    var existedUsername = newUsers.Where(x => x.Username == certificationNumber).FirstOrDefault();
                                    if(existedUsername != null)
                                    {
                                        listErrorUsername.Add(certificationNumber);
                                        //throw new ArgumentException($"{existedUsername.Fullname} và {user.Fullname} trùng số chứng chỉ hành nghề {certificationNumber}");
                                    }
                                    user.Username = certificationNumber;
                                }

                                user.IssueDate = issueDate;
                                user.OrganizationId = Guid.Parse(Default.OrganizationId);
                                user.Type = UserType.INTERNAL;

                                var passwordHasher = new PasswordHasher<User>();
                                user.Password = passwordHasher.HashPassword(user, Default.Password);
                                string nameTitle = worksheet.Cell(i, 6).Value.ToString().Trim();
                                string nameDepartment = worksheet.Cell(i, 7).Value.ToString().Trim();

                                var department = newDepartments.Where(x => nameDepartment.Contains(x.Name)).FirstOrDefault();
                                var title = newTitles.Where(x => nameTitle.Contains(x.Name)).FirstOrDefault();

                                if (title == null)
                                {
                                    title = new Title();
                                    title.Id = Guid.NewGuid();
                                    title.Name = nameTitle;

                                    newTitles.Add(title);
                                    await _dataContext.Titles.AddAsync(title);
                                }

                                if (department == null)
                                {

                                    department = new Department();
                                    department.Id = Guid.NewGuid();
                                    department.Name = nameDepartment;
                                    department.OrganizationId = Guid.Parse(Default.OrganizationId);

                                    newDepartments.Add(department);
                                    await _dataContext.Departments.AddAsync(department);
                                }

                                user.TitleId = title.Id;
                                user.DepartmentId = department.Id;
                                await _dataContext.Users.AddAsync(user);

                                newUsers.Add(user);
                                rowUser.Add(i, user);
                            }

                            if(listErrorUsername.Count != 0)
                            {
                                throw new ArgumentException($"Số chứng chỉ bị trùng: {string.Join(", ", listErrorUsername)}");
                            }
                            #endregion

                            #region Add Program, Organization
                            var newTrainingPrograms_HTHN = new List<TrainingProgram>();
                            var newOrganizations = await _dataContext.Organizations.ToListAsync();


                            for (int i = firstColOfTrp; i <= lastCol; i++)
                            {
                                // Add Organizations
                                var orgName = worksheet.Cell(rowOfOrg, i).Value.ToString().Trim();
                                var existedOrg = newOrganizations.Where(x => x.Name == orgName).FirstOrDefault();
                                if (existedOrg == null && !string.IsNullOrEmpty(orgName))
                                {
                                    existedOrg = new Organization
                                    {
                                        Id = Guid.NewGuid(),
                                        Name = orgName
                                    };
                                    newOrganizations.Add(existedOrg);
                                    await _dataContext.Organizations.AddAsync(existedOrg);
                                }

                                // Add Training Programs
                                DateTime fromDate = DateTime.Now;
                                DateTime toDate = DateTime.Now;

                                DateTime.TryParse(worksheet.Cell(rowOfFromDate, i).Value.ToString().Trim(), out fromDate);
                                DateTime.TryParse(worksheet.Cell(rowOfToDate, i).Value.ToString().Trim(), out toDate);

                                var newTrp = new TrainingProgram
                                {
                                    Id = Guid.NewGuid(),
                                    Name = worksheet.Cell(1, i).Value.ToString(),
                                    Status = TrainingProgramStatus.Initial,
                                    FromDate = fromDate,
                                    ToDate = toDate,
                                    Year = toDate.Year,
                                    TrainingFormId = Guid.Parse(Default.TrainingFormId_HTHN),
                                    OrganizationId = existedOrg?.Id,
                                    MetaDataObject = new TrainingProgramMetaDataObject
                                    {
                                        TrainingSubjects = new List<TrainingSubjectObject> {
                                            new TrainingSubjectObject
                                            {
                                                Amount = 4,
                                                Name = Default.TrainingSubjectName_Participant_HTHN
                                            },
                                            new TrainingSubjectObject
                                            {
                                                Amount = 8,
                                                Name = Default.TrainingSubjectName_Owner_HTHN
                                            }
                                        }
                                    }
                                };
                                newTrainingPrograms_HTHN.Add(newTrp);
                                await _dataContext.TrainingPrograms.AddAsync(newTrp);

                                colTrainingProgram.Add(i, newTrp);
                            }
                            #endregion

                            #region Add User to Program HTHN
                            for (var ic = firstColOfTrp; ic <= lastCol; ic++)
                            {
                                for (var ir = firstRowOfUser; ir < lastRow; ir++)
                                {
                                    int amountNumber = -1;
                                    var amountStr = worksheet.Cell(ir, ic).Value.ToString();
                                    if (Int32.TryParse(amountStr, out amountNumber))
                                    {
                                        if (amountNumber != -1)
                                        {
                                            var newTrp_User = new TrainingProgram_User
                                            {
                                                Id = Guid.NewGuid(),
                                                UserId = rowUser[ir].Id,
                                                TrainingProgramId = colTrainingProgram[ic].Id,
                                                TrainingSubjectName = Default.TrainingSubjectName_Participant_HTHN,
                                                Amount = amountNumber,
                                                Active = true,
                                                Year = colTrainingProgram[ic].Year
                                            };
                                            _dataContext.TrainingProgram_Users.Add(newTrp_User);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Add User to Program NCKH
                            var newTrainingPrograms_DTDH = new List<TrainingProgram>();
                            for (var ir = firstRowOfUser; ir <= lastRow; ir++)
                            {
                                var dtdh = worksheet.Cell(ir, rowOfDTDN).Value.ToString();
                                if (!string.IsNullOrEmpty(dtdh))
                                {
                                    var existedTrpDTDH = newTrainingPrograms_DTDH.Where(x => x.Name == dtdh).FirstOrDefault();
                                    
                                    if(existedTrpDTDH == null)
                                    {
                                        existedTrpDTDH = new TrainingProgram
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = dtdh,
                                            TrainingFormId = Guid.Parse(Default.TrainingFormId_DTDH),
                                            FromDate = new DateTime(2020,1,1),
                                            ToDate = new DateTime(2020,12,31),
                                            MetaDataObject = new TrainingProgramMetaDataObject
                                            {
                                                TrainingSubjects = new List<TrainingSubjectObject> {
                                                    new TrainingSubjectObject
                                                    {
                                                        Amount = Default.TrainingSubjectAmount_Participant_DTDH,
                                                        Name = Default.TrainingSubjectName_Participant_DTDH
                                                    }
                                                }
                                            }
                                        };
                                        newTrainingPrograms_DTDH.Add(existedTrpDTDH);
                                        await _dataContext.TrainingPrograms.AddAsync(existedTrpDTDH);
                                    }

                                    var newTrp_User_DTDH = new TrainingProgram_User
                                    {
                                        Id = Guid.NewGuid(),
                                        UserId = rowUser[ir].Id,
                                        TrainingProgramId = existedTrpDTDH.Id,
                                        TrainingSubjectName = Default.TrainingSubjectName_Participant_DTDH,
                                        Amount = Default.TrainingSubjectAmount_Participant_DTDH,
                                        Active = true,
                                        Year = 2020 // TODO: Sửa fix cứng
                                    };
                                    _dataContext.TrainingProgram_Users.Add(newTrp_User_DTDH);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            throw new ArgumentException($"Sheet {requestModel.SheetName} không tồn tại");
                        }

                        await _dataContext.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ArgumentException(ex.Message);
                    }
                }
            });
        }

    }
}
