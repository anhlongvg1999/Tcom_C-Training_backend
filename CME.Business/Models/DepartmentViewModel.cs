using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class DepartmentViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public Guid OrganizationId { get; set; }

        public OrganizationViewModel Organization { get; set; }

        public DateTime? LastModifiedOnDate { get; set; }
    }
}
