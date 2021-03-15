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
    public class DepartmentService : IDepartmentService
    {
        private const string CachePrefix = "department-";
        private readonly DataContext _dataContext;
        //private readonly ICacheService _cacheService;

        //TODO: CACHE
        public DepartmentService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Pagination<DepartmentViewModel>> GetAllAsync(DepartmentQueryModel queryModel)
        {
            var query = from dept in _dataContext.Departments.Include(x => x.Organization).AsNoTracking()
                        select new DepartmentViewModel
                        {
                            Id = dept.Id,
                            Name = dept.Name,
                            Code = dept.Code,
                            OrganizationId = dept.OrganizationId,
                            Organization = AutoMapperUtils.AutoMap<Organization, OrganizationViewModel>(dept.Organization),
                            LastModifiedOnDate = dept.LastModifiedOnDate
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

            if(queryModel.OrganizationId != null && queryModel.OrganizationId != Guid.Empty)
            {
                query = query.Where(x => x.OrganizationId == queryModel.OrganizationId);
            }

            return await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);
        }

        public async Task<Department> GetById(Guid id)
        {
            var model = await _dataContext.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);     
            return model;
        }

        public async Task<Department> SaveAsync(Department model)
        {
            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                // TODO: USER_ID
                //model.CreatedByUserId = userId;
                model.CreatedOnDate = DateTime.Now;
                model.LastModifiedOnDate = DateTime.Now;

                await _dataContext.Departments.AddAsync(model);
            }
            else
            {
                model.LastModifiedOnDate = DateTime.Now;
                // TODO: USER_ID
                // model.LastModifiedByUserId = actorId;
                _dataContext.Departments.Update(model);
            }
            await _dataContext.SaveChangesAsync();

            InvalidCache(model.Id);

            return model;
        }

        public async Task<bool> DeleteManyAsync(Guid[] deleteIds)
        {
            foreach (var id in deleteIds)
            {
                var department = await GetById(id);

                if (department == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                var deleteTitle = new Department() { Id = id };
                _dataContext.Departments.Attach(deleteTitle);
                _dataContext.Departments.Remove(deleteTitle);
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
