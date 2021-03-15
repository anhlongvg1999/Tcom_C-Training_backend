using CME.Business.Models;
using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface IAuthenticationService
    {

        Task<LoginResponseModel> Login(string username, string password);
        Task<bool> ChangePassword(Guid userId, string oldPassword, string newPassword);
    }
}
