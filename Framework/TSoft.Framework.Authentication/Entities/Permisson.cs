using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    [Table("tblPermission")]
    public class Permisson : BaseTable<Guid>
    {
        public string Prefix { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
