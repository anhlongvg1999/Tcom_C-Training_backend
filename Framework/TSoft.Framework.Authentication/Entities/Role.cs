using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    [Table("tblRole")]
    public class Role : BaseTable<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
    }
}
