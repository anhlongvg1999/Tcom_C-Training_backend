using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TSoft.Framework.Authentication
{
    public class UserRequestModel
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Gender { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public string AvatarUrl { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDay { set; get; }
        public List<Guid>? RoleIds { set; get; }


    }
}
