using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.Authentication
{
    public class RoleRequestModel
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public Guid[] PermissionIds { get; set; }
    }
}
