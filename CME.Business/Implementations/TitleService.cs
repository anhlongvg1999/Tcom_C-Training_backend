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
    public class TitleService : ITitleService
    {
        private const string CachePrefix = "title-";
        private readonly DataContext _dataContext;
        //private readonly ICacheService _cacheService;

        //TODO: CACHE
        public TitleService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Pagination<TitleViewModel>> GetAllAsync(TitleQueryModel queryModel)
        {
            var query = from tit in _dataContext.Titles.AsNoTracking()
                        select new TitleViewModel
                        {
                            Id = tit.Id,
                            Name = tit.Name,
                            Code = tit.Code,
                            LastModifiedOnDate = tit.LastModifiedOnDate
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

            return await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);
        }

        public async Task<Title> GetById(Guid id)
        {
            var model = await _dataContext.Titles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return model;
        }

        public async Task<Title> SaveAsync(Title model)
        {
            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                // TODO: USER_ID
                //model.CreatedByUserId = userId;
                model.CreatedOnDate = DateTime.Now;
                model.LastModifiedOnDate = DateTime.Now;

                await _dataContext.Titles.AddAsync(model);
            }
            else
            {
                model.LastModifiedOnDate = DateTime.Now;
                // TODO: USER_ID
                // model.LastModifiedByUserId = actorId;
                _dataContext.Titles.Update(model);
            }
            await _dataContext.SaveChangesAsync();

            InvalidCache(model.Id);

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

                var deleteTitle = new Title() { Id = id };
                _dataContext.Titles.Attach(deleteTitle);
                _dataContext.Titles.Remove(deleteTitle);
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
