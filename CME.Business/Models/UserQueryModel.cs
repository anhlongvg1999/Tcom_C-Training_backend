using System;
using System.Collections.Generic;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Business.Models
{
    public class UserQueryModel : PaginationRequest
    {
        public Guid OrganizationId;

        public Guid DepartmentId;

        public Guid TitleId;

        public int? Year;
    }
}
