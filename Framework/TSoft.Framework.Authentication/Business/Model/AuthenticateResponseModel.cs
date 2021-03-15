using System;
using System.Collections.Generic;
using System.Text;
using TSoft.Framework.Authentication;

namespace TSoft.Framework.ApiUtils.Business.Model
{
    public class AuthenticateResponseModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Gender { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime? BirthDay { set; get; }
        public string Token { get; set; }
        public List<string> Rights { get; set; }



        public AuthenticateResponseModel(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Fullname = user.Fullname;
            AvatarUrl = user.AvatarUrl;
            Email = user.Email;
            Token = token;
        }
    }
}
