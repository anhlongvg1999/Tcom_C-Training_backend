using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Entities
{
    [Table("auth_Users")]
    public class User : BaseTable<User>
    {
        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Firstname { get; set; }

        public string Fullname
        {
            get => $"{Lastname} {Firstname}";
            set
            {
                var fullname = value;
                var words = fullname.Trim().Split(' ');
                Firstname = words[words.Length - 1];

                if (words.Length > 1)
                {
                    var lengthLastname = fullname.Length - Firstname.Length;
                    Lastname = fullname.Substring(0, lengthLastname).Trim();
                }
            }
        }

        public string Lastname { get; set; }

        public string Code { get; set; }

        public string CertificateNumber { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Email { get; set; }

        public int? Gender { get; set; } // 0: Nữ, 1: Nam

        public string Address { get; set; }

        public Guid? TitleId { get; set; }

        public string Type { get; set; }

        public string AvatarUrl { get; set; }

        [ForeignKey("TitleId")]
        public Title Title { get; set; }

        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        public Guid? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
