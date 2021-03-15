using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    [Table("tlbRolePermisson")]
    public class RolePermisson : BaseTable<Guid>
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
