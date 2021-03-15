using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class DepartmentRequestModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public Guid OrganizationId { get; set; }
    }
}
