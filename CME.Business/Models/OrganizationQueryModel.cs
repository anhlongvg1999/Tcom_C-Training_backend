using System;
using System.Collections.Generic;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Business.Models
{
    public class DepartmentQueryModel : PaginationRequest
    {
        public Guid OrganizationId;
    }
}
