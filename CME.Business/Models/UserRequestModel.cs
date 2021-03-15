using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class UserRequestModel
    {
        public string Username { get; set; }

        public string Fullname { get; set; }

        public string Code { get; set; }

        public string CertificateNumber { get; set; }

        public string IdentificationNumber { get; set; }

        public string IssueDate { get; set; }

        public string? BirthDate { get; set; }

        public string Email { get; set; }

        public int? Gender { get; set; } // 0: Nữ, 1: Nam

        public string Address { get; set; }

        public string Type { get; set; } // Internal / External

        public Guid? TitleId { get; set; }

        public Guid? OrganizationId { get; set; }

        public Guid? DepartmentId { get; set; }

        public IFormFile AvatarFile { get; set; }
    }
}
