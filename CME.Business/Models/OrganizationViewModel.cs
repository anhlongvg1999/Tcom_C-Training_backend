using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class OrganizationViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public DateTime? LastModifiedOnDate { get; set; }
    }
}
