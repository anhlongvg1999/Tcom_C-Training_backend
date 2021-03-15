using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.ApiUtils.Business.Model;
using TSoft.Framework.Authentication.Business.Model;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    public interface IUserService
    {
        public const string Message_UserOrPasswordIsWrong = "user-or-password-is-wrong";
        public const string Message_UserNotFound = "UserNotFound";
        public const string Message_UserNotPermisson = "UserNotPermission";
        public const string Message_EmployeeExisted = "EmployeeExisted";
        public const string Message_UserNameAlreadyExist = "UserNameAlreadyExist";
        Task<Pagination<User>> GetFilterAsync(UserQueryModel queryModel);
        Task<User> GetById(Guid Id);
        Task<User> SaveAsync(User appUser, List<Guid> roleIds, Guid employeeId);
        Task<User> UpdateAsync(User appUser, List<Guid> roleIds);
        Task<bool> AddRole(Guid Id, List<Guid> roleIds);
        Task<List<Role>> GetRole(Guid Id);
        Task<List<Permisson>> GetPermission(Guid Id);
        Task<AuthenticateResponseModel> Login(AuthenticateRequestModel model);
        Task<User> ChangePassword(Guid Id, ChangePasswordRequestModel data);
        Task<bool> CheckSendMail(CheckCodeViewModel data);
    }
}
