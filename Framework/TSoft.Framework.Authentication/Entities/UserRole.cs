using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    [Table("tblUserRole")]
    public class UserRole : BaseTable<Guid>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
