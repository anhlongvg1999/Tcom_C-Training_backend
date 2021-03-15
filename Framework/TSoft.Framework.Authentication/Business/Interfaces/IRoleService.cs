using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.ApiUtils.Business.Model;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{ 
    public interface IRoleService
    {
        public const string Message_RoleNotFound = "RoleNotFound";
        Task<Pagination<Role>> GetFilterAsync(RoleQueryModel queryModel);
        Task<AppRoleDetailModel> GetById(Guid Id);
        Task<List<Permisson>> GetPermisson(Guid Id);
        Task<Role> SaveAsync(Role appRole, Guid[] permissionIds);
        Task<Role> UpdateAsync(Role appRole, Guid[] permissionIds);
        Task<bool> AddPermission(Guid Id, Guid[] permissionIds);
        Task<bool> DeleteManyAsync(Guid[] deleteIds);
    }
}
