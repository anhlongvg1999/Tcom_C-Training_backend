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

namespace CME.Business.Implementations
{
    public class TrainingFormService : ITrainingFormService
    {
        private const string CachePrefix = "trainingForm-";
        private readonly DataContext _dataContext;
        //private readonly ICacheService _cacheService;

        //TODO: CACHE
        public TrainingFormService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Pagination<TrainingFormViewModel>> GetAllAsync(TrainingFormQueryModel queryModel)
        {
            var query = from tr in _dataContext.TrainingForms.AsNoTracking()
                        select new TrainingFormViewModel
                        {
                            Id = tr.Id,
                            Name = tr.Name,
                            Code = tr.Code,
                            TrainingSubjects = _dataContext.TrainingSubjects.Where(x => x.TrainingFormId == tr.Id).OrderBy(x => x.Order).ToList(),
                            LastModifiedOnDate = tr.LastModifiedOnDate
                        };
            if (queryModel.ListTextSearch != null && queryModel.ListTextSearch.Count > 0)
            {
                foreach (var ts in queryModel.ListTextSearch)
                {
                    query = query.Where(q =>
                        q.Name.Contains(ts) ||
                        q.Code.Contains(ts)
                    );
                }
            }

            var result = await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);

            return result;
        }

        public async Task<TrainingFormViewModel> GetById(Guid id)
        {
            var model = await _dataContext.TrainingForms.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            var result = AutoMapperUtils.AutoMap<TrainingForm, TrainingFormViewModel>(model);
            result.TrainingSubjects = await _dataContext.TrainingSubjects.AsNoTracking().Where(x => x.TrainingFormId == id).OrderBy(x => x.Order).ToListAsync();
            return result;
        }

        public async Task<TrainingForm> SaveAsync(TrainingForm model, ICollection<TrainingSubject> trainingSubjects)
        {
            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                // TODO: USER_ID
                //model.CreatedByUserId = userId;
                model.CreatedOnDate = DateTime.Now;
                model.LastModifiedOnDate = DateTime.Now;

                await _dataContext.TrainingForms.AddAsync(model);
            }
            else
            {
                model.LastModifiedOnDate = DateTime.Now;
                // TODO: USER_ID
                // model.LastModifiedByUserId = actorId;
                _dataContext.TrainingForms.Update(model);

                var deleteTrainingSubjects = _dataContext.TrainingSubjects.Where(x => x.TrainingFormId == model.Id);
                _dataContext.TrainingSubjects.RemoveRange(deleteTrainingSubjects);
            }

            if (trainingSubjects != null && trainingSubjects.Count > 0)
            {
                trainingSubjects = trainingSubjects.Select(x =>
                {
                    x.Id = Guid.NewGuid();
                    x.TrainingFormId = model.Id;
                    //x.CreatedByUserId = userId;
                    x.CreatedOnDate = DateTime.Now;
                    //x.LastModifiedByUserId = actorId;
                    x.LastModifiedOnDate = DateTime.Now;
                    return x;
                }).ToList();

                await _dataContext.TrainingSubjects.AddRangeAsync(trainingSubjects);
            }

            await _dataContext.SaveChangesAsync();

            InvalidCache(model.Id);

            //TODO: Xử lý loop bên trong dữ liệu
            model.TrainingSubjects = null;

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

                var deleteTrainingForm = new TrainingForm() { Id = id };
                _dataContext.TrainingForms.Attach(deleteTrainingForm);
                _dataContext.TrainingForms.Remove(deleteTrainingForm);
            }
            await _dataContext.SaveChangesAsync();
            return true;
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
