using AutoMapper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TSoft.Framework.ApiUtils.Business.Model;
using TSoft.Framework.Authentication.Business;
using TSoft.Framework.Authentication.Business.Interfaces;
using TSoft.Framework.Authentication.Business.Model;
using TSoft.Framework.CacheMemory;
using TSoft.Framework.CacheMemory.Interfaces;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    public class UserService : IUserService
    {
        private const string CachePrefix = "user-";
        private readonly DataContextBase _dataContext;
        private IEmailCodeService _emailCode;
        private IEmailConfiguration _emailConfiguration;
        private ICacheBase _cache;
        public UserService(DataContextBase dataContext,
           IEmailCodeService emailCode,
           ICacheBase cache,
        IEmailConfiguration emailConfiguration)
        {
            _dataContext = dataContext;
            _emailCode = emailCode;
            _cache = cache;
            _emailConfiguration = emailConfiguration;

            // _config = config;
        }


        public async Task<Pagination<User>> GetFilterAsync(UserQueryModel queryModel)
        {
            queryModel.PageSize = queryModel.PageSize.HasValue ? queryModel.PageSize.Value : 20;
            queryModel.CurrentPage = queryModel.CurrentPage.HasValue ? queryModel.CurrentPage.Value : 1;

            var query = _dataContext.AppUsers.AsNoTracking();

            if (queryModel.ListTextSearch != null && queryModel.ListTextSearch.Count > 0)
            {
                foreach (var ts in queryModel.ListTextSearch)
                {
                    query = query.Where(q =>
                        q.Username.Contains(ts) ||
                        q.Email.Contains(ts)
                    );
                }
            }

            return await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);
        }

        public async Task<User> GetById(Guid Id)
        {
            var cacheKey = BuildCacheKey(Id);
            return await _cache.GetOrCreate(cacheKey, async () =>
            {
                var user = _dataContext.AppUsers.FirstOrDefault(x => x.Id == Id);
                return user;
            });
           
        }
        public async Task<List<Role>> GetRole(Guid Id)
        {
            var query = from ar in _dataContext.UserRoles.AsNoTracking()
                        join r in _dataContext.AppRoles.AsNoTracking() on ar.RoleId equals r.Id
                        //where ar.UserId == Id && r.Status == Status.Active
                        select new Role
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Code = r.Code,
                            IsActive = r.IsActive
                        };
            //var query = from ar in _dataContext.UserRoles.AsNoTracking()
            //            join r in _dataContext.AppRoles.AsNoTracking() on ar.RoleId equals r.Id
            //            //where ar.UserId == Id && r.Status == Status.Active
            //            select r;
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<Permisson>> GetPermission(Guid userId)
        {
            //var appRoles = GetRole(Id).Result;
            var lstRole = from ar in _dataContext.UserRoles.AsNoTracking()
                          join r in _dataContext.AppRoles.AsNoTracking() on ar.RoleId equals r.Id
                          where ar.UserId == userId
                          select new Role
                          {
                              Id = r.Id,
                              Name = r.Name,
                              Code = r.Code,
                              IsActive = r.IsActive
                          };

            var query = from rp in _dataContext.RolePermissons
                        join a in lstRole on rp.RoleId equals a.Id
                        join p in _dataContext.Permissons on rp.PermissionId equals p.Id
                        select p;
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<User> SaveAsync(User appUser, List<Guid> roleIds, Guid employeeId)
        {
            var userName = _dataContext.AppUsers.AsNoTracking().FirstOrDefault(x => x.Username == appUser.Username);
            if (userName != null)
            {
                throw new ArgumentException(IUserService.Message_UserNameAlreadyExist);
            }
            if (appUser.Email != null)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(appUser.Email);
                if (!match.Success)
                {
                    throw new ArgumentException("Email incorrect format");
                }
            }

            appUser.VerificationCode = RandomString(7);
            appUser.CreatedOnDate = DateTime.Now;
            appUser.LastModifiedOnDate = DateTime.Now;

            var passwordHasher = new PasswordHasher<User>();
            appUser.Password = passwordHasher.HashPassword(appUser, appUser.Password);

            await _dataContext.AppUsers.AddAsync(appUser);

            if (appUser.Email != null)
            {
                if (_emailConfiguration.Enable == true)
                {
                    _emailCode.SendMail(appUser.Email, appUser.VerificationCode);
                }

            }
            if (roleIds != null)
            {
                await AddRole(appUser.Id, roleIds);
            }
            _dataContext.SaveChanges();
            return appUser;
        }

        public async Task<User> UpdateAsync(User appUser, List<Guid> roleIds)
        {
            var entity = _dataContext.AppUsers.AsNoTracking().FirstOrDefault(x => x.Id == appUser.Id);
            if (entity == null)
            {
                throw new ArgumentException(IUserService.Message_UserNotFound);
            }
            appUser.Password = entity.Password;
            appUser.LastModifiedOnDate = DateTime.Now;
            _dataContext.AppUsers.Update(appUser);

            _dataContext.SaveChanges();
            InvalidCache(appUser.Id);
            if (roleIds != null)
            {
                await AddRole(appUser.Id, roleIds);
            }

            return appUser;
        }
        public async Task<User> ChangePassword(Guid Id, ChangePasswordRequestModel data)
        {
            var user = await _dataContext.AppUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                throw new ArgumentException(IUserService.Message_UserNotFound);
            }
            bool verified = false;
            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, data.Password);
            if (result == PasswordVerificationResult.Success) verified = true;
            else if (result == PasswordVerificationResult.SuccessRehashNeeded) verified = true;
            else if (result == PasswordVerificationResult.Failed) verified = false;

            if (verified == false)
            {
                throw new Exception(IUserService.Message_UserOrPasswordIsWrong);
            }

            user.Password = passwordHasher.HashPassword(user, data.PasswordNew);
            user.LastModifiedOnDate = DateTime.Now;
            _dataContext.AppUsers.Update(user);

            _dataContext.SaveChanges();
            return user;
        }

        public async Task<bool> AddRole(Guid Id, List<Guid> roleIds)
        {
            var oldData = _dataContext.UserRoles.Where(x => x.UserId == Id);
            _dataContext.UserRoles.RemoveRange(oldData);

            foreach (var roleId in roleIds)
            {
                var userRole = new UserRole();
                userRole.UserId = Id;
                userRole.RoleId = roleId;
                await _dataContext.UserRoles.AddAsync(userRole);
            }
            _dataContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteManyAsync(Guid[] deleteIds)
        {
            foreach (var Id in deleteIds)
            {
                var user = await _dataContext.AppUsers
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (user == null)
                {
                    throw new ArgumentException(IUserService.Message_UserNotFound);
                }
                _dataContext.AppUsers.Update(user);
                InvalidCache(Id);
            }

            await _dataContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CheckSendMail(CheckCodeViewModel data)
        {
            var user = _dataContext.AppUsers.Where(x => x.Id == data.UserId && x.VerificationCode == data.VerificationCode).FirstOrDefault();
            if (user == null)
            {
                user.IsActive = false;
                _dataContext.AppUsers.Update(user);
                _dataContext.SaveChanges();
                return false;
            }
            user.IsActive = true;
            _dataContext.AppUsers.Update(user);
            _dataContext.SaveChanges();
            return true;

        }

        public async Task<AuthenticateResponseModel> Login(AuthenticateRequestModel model)
        {
            var user = _dataContext.AppUsers.Where(x => x.Username == model.Username).FirstOrDefault();
            if (user == null)
            {
                throw new Exception(IUserService.Message_UserOrPasswordIsWrong);
            }
            bool verified = false;
            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
            if (result == PasswordVerificationResult.Success) verified = true;
            else if (result == PasswordVerificationResult.SuccessRehashNeeded) verified = true;
            else if (result == PasswordVerificationResult.Failed) verified = false;

            if (verified == false)
            {
                throw new Exception(IUserService.Message_UserOrPasswordIsWrong);
            }

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            var authenResponse = new AuthenticateResponseModel(user, token);

            var query = (from ur in _dataContext.UserRoles
                         join rp in _dataContext.RolePermissons on ur.RoleId equals rp.RoleId
                         join p in _dataContext.Permissons on rp.PermissionId equals p.Id
                         select p.Key).Distinct();
            authenResponse.Rights = query.ToList();
            return authenResponse;
        }

        private string generateJwtToken(User user)
        {

            var claims = new[]
                    {
                    new Claim("Username", user.Username),
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    };
            var tonkenPrefix = "372F78BC6B66F3CEAF705FE57A91F369A5BE956692A4DA7DE16CAD71113CF046";
            var issuer = "Tsoft";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tonkenPrefix));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer,
                issuer,
                claims,
                expires: DateTime.UtcNow.AddMonths(3),
                signingCredentials: creds);

            var token_access = new JwtSecurityTokenHandler().WriteToken(token);
            return token_access;

        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void InvalidCache(Guid id)
        {
            string cacheKey = BuildCacheKey(id);
            string cacheMasterKey = BuildCacheMasterKey();

            _cache.Remove(cacheKey);
            _cache.Remove(cacheMasterKey);
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
