using System;
using System.Collections.Generic;
using System.Text;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    public class AppRoleDetailModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public List<Permisson> Permissons { get; set; }
    }
}
