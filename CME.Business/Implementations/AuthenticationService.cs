using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SERP.Filenet.DB;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tsoft.Framework.Common;
using Tsoft.Framework.Common.Configs;

namespace CME.Business.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService; 

        public AuthenticationService(DataContext dataContext, IConfiguration configuration, IUserService userService)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<LoginResponseModel> Login(string username, string password)
        {
            var user = await _userService.GetByUsername(username);
            if (user == null)
            {
                throw new ArgumentException($"Tên tài khoản hoặc mật khẩu không đúng");
            }

            if (!VerifyPassword(user, password))
            {
                throw new ArgumentException($"Tên tài khoản hoặc mật khẩu không đúng");
            }

            var token = GenerateJwtToken(user);
            var userVM = AutoMapperUtils.AutoMap<User, UserViewModel>(user);
            var response = new LoginResponseModel { User = userVM, Token = token };
            return response;
        }

        public async Task<bool> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userService.GetById(userId);
            if (user == null)
            {
                throw new ArgumentException($"Tài khoản không tồn tại");
            }

            if (!VerifyPassword(user, oldPassword))
            {
                throw new ArgumentException($"Mật khẩu cũ không đúng");
            }
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, newPassword);

            await _userService.SaveAsync(user, null);

            return true;
        }

        private bool VerifyPassword(User user, string password)
        {
            bool verified = false;
            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success) verified = true;
            else if (result == PasswordVerificationResult.SuccessRehashNeeded) verified = true;
            else if (result == PasswordVerificationResult.Failed) verified = false;
            return verified;
        }

        private string GenerateJwtToken(User user)
        {
            string roles = "";
            if (user.Roles != null)
            {
                roles = string.Join(",", user.Roles.Select(x => x.Code).ToList());
            }
            var claims = new[]
                    {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.Username),
                    new Claim("Fullname", user.Fullname),
                    new Claim("Roles", roles),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    };
            var tonkenPrefix = _configuration.GetSection(nameof(JWTAuthenticaion)).Get<JWTAuthenticaion>();
            var issuer = "Tsoft";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tonkenPrefix.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer,
                issuer,
                claims,
                expires: DateTime.UtcNow.AddMonths(3),
                signingCredentials: creds);

            var token_access = new JwtSecurityTokenHandler().WriteToken(token);
            return token_access;

        }
    }
}
