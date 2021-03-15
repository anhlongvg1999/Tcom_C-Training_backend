using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsoft.Framework.Common;
using TSoft.Framework.ApiUtils.Business.Model;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    public class RoleService : IRoleService
    {
        private readonly DataContextBase _dataContext;
        public RoleService(DataContextBase dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Pagination<Role>> GetFilterAsync(RoleQueryModel queryModel)
        {
            queryModel.PageSize = queryModel.PageSize.HasValue ? queryModel.PageSize.Value : 20;
            queryModel.CurrentPage = queryModel.CurrentPage.HasValue ? queryModel.CurrentPage.Value : 1;

            var query = _dataContext.AppRoles.AsNoTracking();

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

        public async Task<AppRoleDetailModel> GetById(Guid Id)
        {
            var entity = _dataContext.AppRoles.AsNoTracking().FirstOrDefault(x => x.Id == Id);
            if (entity == null)
            {
                throw new Exception(IRoleService.Message_RoleNotFound);
            }
            var result = AutoMapperUtils.AutoMap<Role, AppRoleDetailModel>(entity);
            result.Permissons = await GetPermisson(Id);

            return result;
        }

        public async Task<List<Permisson>> GetPermisson(Guid Id)
        {
            var query = from rp in _dataContext.RolePermissons
                        join p in _dataContext.Permissons on rp.PermissionId equals p.Id
                        where rp.RoleId == Id
                        select p;
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<Role> SaveAsync(Role appRole, Guid[] permissionIds)
        {
            appRole.CreatedOnDate = DateTime.Now;
            appRole.LastModifiedOnDate = DateTime.Now;
            await _dataContext.AppRoles.AddAsync(appRole);
            _dataContext.SaveChanges();

            await AddPermission(appRole.Id, permissionIds);
            return appRole;
        }

        public async Task<Role> UpdateAsync(Role appRole, Guid[] permissionIds)
        {
            appRole.LastModifiedOnDate = DateTime.Now;
            _dataContext.AppRoles.Update(appRole);
            _dataContext.SaveChanges();

            await AddPermission(appRole.Id, permissionIds);
            return appRole;
        }
        public async Task<bool> AddPermission(Guid Id, Guid[] permissionIds)
        {
            var oldData = _dataContext.RolePermissons.Where(x => x.RoleId == Id);
            _dataContext.RolePermissons.RemoveRange(oldData);

            foreach (var permissionId in permissionIds)
            {
                var rolePermisson = new RolePermisson();
                rolePermisson.PermissionId = permissionId;
                rolePermisson.RoleId = Id;
                await _dataContext.RolePermissons.AddAsync(rolePermisson);
            }
            _dataContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteManyAsync(Guid[] deleteIds)
        {
            foreach (var Id in deleteIds)
            {
                var roles = await _dataContext.AppRoles
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (roles == null)
                {
                    throw new ArgumentException(IRoleService.Message_RoleNotFound);
                }
                _dataContext.AppRoles.Update(roles);
            }
            await _dataContext.SaveChangesAsync();
            return true;
        }

    }
}
