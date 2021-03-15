using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Fullname { get; set; }

        public string Code { get; set; }

        public string CertificateNumber { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Email { get; set; }

        public int? Gender { get; set; } // 0: Nữ, 1: Nam

        public string Address { get; set; }

        public string Type { get; set; } // Internal / External

        public string AvatarUrl { get; set; }

        public Guid? TitleId { get; set; }

        public Title Title { get; set; }

        public Guid? OrganizationId { get; set; }

        public Organization Organization { get; set; }

        public Guid? DepartmentId { get; set; }

        public Department Department { get; set; }

        public int AmoutInYear { get; set; }

        public DateTime? LastModifiedOnDate { get; set; }
    }
}
