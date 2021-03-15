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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using CME.Entities.Constants;
using System.IO.Compression;

namespace CME.Business.Implementations
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private const string CachePrefix = "trainingProgram-";
        private readonly DataContext _dataContext;
        private readonly IHostingEnvironment _environment;
        //private readonly ICacheService _cacheService;

        //TODO: CACHE
        public TrainingProgramService(DataContext dataContext, IHostingEnvironment environment)
        {
            _dataContext = dataContext;
            _environment = environment;
        }

        public async Task<Pagination<TrainingProgramViewModel>> GetAllAsync(TrainingProgramQueryModel queryModel)
        {
            var query = from trp in _dataContext.TrainingPrograms.AsNoTracking().Include(x => x.Organization).Include(x => x.TrainingForm)
                        select new TrainingProgramViewModel
                        {
                            Id = trp.Id,
                            Name = trp.Name,
                            Code = trp.Code,
                            FromDate = trp.FromDate,
                            ToDate = trp.ToDate,
                            OrganizationId = trp.OrganizationId,
                            Organization = trp.Organization,
                            TrainingFormId = trp.TrainingFormId,
                            TrainingForm = trp.TrainingForm,
                            Address = trp.Address,
                            Note = trp.Note,
                            Status = trp.Status,
                            MetaDataObject = trp.MetaDataObject,
                            LastModifiedOnDate = trp.LastModifiedOnDate
                        };

            if (queryModel.ListTextSearch != null && queryModel.ListTextSearch.Count > 0)
            {
                foreach (var ts in queryModel.ListTextSearch)
                {
                    query = query.Where(q =>
                        q.Name.Contains(ts) ||
                        q.Code.Contains(ts) ||
                        q.Address.Contains(ts)
                    );
                }
            }

            if (queryModel.FromDate.HasValue)
            {
                var fromDate = new DateTime(queryModel.FromDate.Value.Year, queryModel.FromDate.Value.Month, queryModel.FromDate.Value.Day);
                query = query.Where(x => x.FromDate >= fromDate);
            }
            if (queryModel.ToDate.HasValue)
            {
                var toDate = new DateTime(queryModel.ToDate.Value.Year, queryModel.ToDate.Value.Month, queryModel.ToDate.Value.Day).AddDays(1);
                query = query.Where(x => x.ToDate < toDate);
            }


            if (queryModel.TrainingFormId != null)
            {
                query = query.Where(x => x.TrainingFormId == queryModel.TrainingFormId);
            }

            var result = await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);
            return result;
        }

        public async Task<TrainingProgram> GetById(Guid id)
        {
            var model = await _dataContext.TrainingPrograms
                .AsNoTracking()
                .Include(x => x.Organization)
                .Include(x => x.TrainingForm).Include(x => x.TrainingProgram_Users).FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                throw new ArgumentException($"Id {id} không tồn tại");
            }

            var query = from trp_u in _dataContext.TrainingProgram_Users.Where(x => x.TrainingProgramId == model.Id)
                        join user in _dataContext.Users.Include(x => x.Department).Include(x => x.Title) on trp_u.UserId equals user.Id
                        select new TrainingProgram_User
                        {
                            UserId = trp_u.UserId,
                            User = user,
                            TrainingSubjectName = trp_u.TrainingSubjectName,
                            Amount = trp_u.Amount,
                            Active = trp_u.Active
                        };
            model.TrainingProgram_Users = await query.ToListAsync();
            model.TrainingProgram_Users = model.TrainingProgram_Users.OrderBy(x => x.User.Firstname).ThenBy(x => x.User.Lastname).ToList();
            return model;
        }

        public async Task<TrainingProgram> SaveAsync(TrainingProgram model, ICollection<TrainingProgram_UserRequestModel> trainingProgram_UserRequestModel)
        {
            model.Organization = null;
            model.TrainingForm = null;
            model.TrainingProgram_Users = new List<TrainingProgram_User>();

            if (model.ToDate != null)
            {
                model.Year = model.ToDate.Value.Year;
            }

            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                model.Status = TrainingProgramStatus.Initial;
                // TODO: USER_ID
                //model.CreatedByUserId = userId;
                model.CreatedOnDate = DateTime.Now;
                model.LastModifiedOnDate = DateTime.Now;

                await _dataContext.TrainingPrograms.AddAsync(model);
            }
            else
            {
                model.LastModifiedOnDate = DateTime.Now;
                // TODO: USER_ID
                // model.LastModifiedByUserId = actorId;
                _dataContext.TrainingPrograms.Update(model);

                var deleteTrainingProgram_Users = _dataContext.TrainingProgram_Users.Where(x => x.TrainingProgramId == model.Id);
                _dataContext.TrainingProgram_Users.RemoveRange(deleteTrainingProgram_Users);
            }

            if (trainingProgram_UserRequestModel != null && trainingProgram_UserRequestModel.Count > 0)
            {
                List<TrainingProgram_User> trainingProgram_Users = new List<TrainingProgram_User>();
                foreach (var item in trainingProgram_UserRequestModel)
                {
                    trainingProgram_Users.Add(new TrainingProgram_User
                    {
                        Id = Guid.NewGuid(),
                        TrainingProgramId = model.Id,
                        TrainingSubjectName = item.TrainingSubjectName,
                        UserId = item.UserId,
                        Amount = item.Amount,
                        Active = item.Active,
                        Year = model.Year,
                        //x.CreatedByUserId = userId;
                        CreatedOnDate = DateTime.Now,
                        //x.LastModifiedByUserId = actorId;
                        LastModifiedOnDate = DateTime.Now
                    });
                }
                await _dataContext.TrainingProgram_Users.AddRangeAsync(trainingProgram_Users);
            }
            await _dataContext.SaveChangesAsync();

            InvalidCache(model.Id);

            //TODO: Xử lý loop bên trong dữ liệu
            model.TrainingProgram_Users = null;

            return model;
        }

        public async Task<bool> DeleteManyAsync(Guid[] deleteIds)
        {
            foreach (var id in deleteIds)
            {
                var model = await GetById(id);

                if (model == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                var deleteTrainingProgram = new TrainingProgram() { Id = id };
                _dataContext.TrainingPrograms.Attach(deleteTrainingProgram);
                _dataContext.TrainingPrograms.Remove(deleteTrainingProgram);
            }
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<TrainingProgram_User> Checkin(Guid TrainingProgramId, Guid UserId, bool Active)
        {
            var model = await _dataContext.TrainingProgram_Users.Where(x => x.UserId == UserId && x.TrainingProgramId == TrainingProgramId).FirstOrDefaultAsync();

            if (model == null)
            {
                throw new ArgumentException($"Không tồn tại bản ghi");
            }

            model.Active = Active;
            _dataContext.Update(model);

            await _dataContext.SaveChangesAsync();
            return model;
        }

        public async Task<MemoryStream> ExportCertifications(Guid id)
        {
            var model = await GetById(id);
            var trp_users = await _dataContext.TrainingProgram_Users.Where(x => x.TrainingProgramId == id && x.Active == true).Include(x => x.User).ToListAsync();
            var listFile = new List<FileInfoModel>();

            var templatePath = Path.Combine(_environment.WebRootPath, "templates", "certificate-2021-01-01.docx");
            using (WordprocessingDocument templateDoc = WordprocessingDocument.Open(templatePath, false))
            {

                foreach (var item in trp_users)
                {
                    var pathToSave = GetCertificationPath(id);
                    if (!Directory.Exists(pathToSave))
                        Directory.CreateDirectory(pathToSave);

                    var filePath = Path.Combine(pathToSave, item.UserId + ".docx");

                    using (var newDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                    {
                        foreach (var part in templateDoc.Parts)
                            newDoc.AddPart(part.OpenXmlPart, part.RelationshipId);
                        MainDocumentPart mainPart = newDoc.MainDocumentPart;
                        Body body = mainPart.Document.Body;

                        foreach (var text in body.Descendants<Text>())
                        {
                            if (text.Text.Contains("[Name]"))
                            {
                                text.Text = text.Text.Replace("[Name]", item.User.Fullname);
                            }
                            if (text.Text.Contains("[Title]"))
                            {


                                text.Text = text.Text.Replace("[Title]", model.Name); //49 kí tự
                            }
                        }
                        newDoc.Save();

                        listFile.Add(new FileInfoModel
                        {
                            FileName = item.User.Fullname + " " + item.User.CertificateNumber.Replace("/", "-") + ".docx",
                            FilePath = filePath
                        });
                    }


                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < listFile.Count; i++)
                    {
                        ziparchive.CreateEntryFromFile(listFile[i].FilePath, listFile[i].FileName);
                    }
                }
                //return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
                var fileName = id + ".zip";
                var fileZipPath = Path.Combine(GetCertificationPath(id), fileName);
                using (var fs = new FileStream(fileZipPath, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }
                return memoryStream;
            }

        }

        private string GetCertificationPath(Guid id)
        {
            return Path.Combine(CertificationPath(), id.ToString());
        }

        public async Task<bool> ChangeStatus(Guid id, string status)
        {
            var model = await GetById(id);
            model.Status = status;
            var trainingProgram_UserRequestModels = new List<TrainingProgram_UserRequestModel>();
            foreach(var trp_u in model.TrainingProgram_Users)
            {
                var trp_ur = AutoMapperUtils.AutoMap<TrainingProgram_User, TrainingProgram_UserRequestModel>(trp_u);
                trainingProgram_UserRequestModels.Add(trp_ur);
            }
            await SaveAsync(model, trainingProgram_UserRequestModels);

            return true;
        }

        public string CertificationPath()
        {
            return Path.Combine(_environment.WebRootPath, "certificate");
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
