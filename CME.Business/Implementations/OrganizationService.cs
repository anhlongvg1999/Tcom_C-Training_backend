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
    public class OrganizationService : IOrganizationService
    {
        private const string CachePrefix = "organization-";
        private readonly DataContext _dataContext;
        //private readonly ICacheService _cacheService;

        //TODO: CACHE
        public OrganizationService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Pagination<OrganizationViewModel>> GetAllAsync(OrganizationQueryModel queryModel)
        {
            var query = from org in _dataContext.Organizations
                        //orderby org.LastModifiedOnDate descending
                        select new OrganizationViewModel
                        {
                            Id = org.Id,
                            Name = org.Name,
                            Code = org.Code,
                            Address = org.Address,
                            LastModifiedOnDate = org.LastModifiedOnDate
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

            return await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);
        }

        public async Task<Organization> GetById(Guid id)
        {
            var model = await _dataContext.Organizations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return model;
        }

        public async Task<Organization> SaveAsync(Organization model)
        {
            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                // TODO: USER_ID
                //model.CreatedByUserId = userId;
                model.CreatedOnDate = DateTime.Now;
                model.LastModifiedOnDate = DateTime.Now;

                await _dataContext.Organizations.AddAsync(model);
            }
            else
            {
                model.LastModifiedOnDate = DateTime.Now;
                // TODO: USER_ID
                // model.LastModifiedByUserId = actorId;
                _dataContext.Organizations.Update(model);
            }
            await _dataContext.SaveChangesAsync();

            InvalidCache(model.Id);

            return model;
        }

        public async Task<bool> DeleteManyAsync(Guid[] deleteIds)
        {
            foreach (var id in deleteIds)
            {
                var organization = await GetById(id);

                if (organization == null)
                {
                    throw new ArgumentException($"Id {organization.Id} không tồn tại");
                }

                var deleteOrganization = new Organization() { Id = id };
                _dataContext.Organizations.Attach(deleteOrganization);
                _dataContext.Organizations.Remove(deleteOrganization);
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
