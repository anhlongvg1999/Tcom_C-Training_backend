using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    [Table("tblUser")]
    public class User : BaseTable<Guid>
    {
        [Required]
        public string Username { get; set; }
        [Required]
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
        public string VerificationCode { get; set; }
    }
}
