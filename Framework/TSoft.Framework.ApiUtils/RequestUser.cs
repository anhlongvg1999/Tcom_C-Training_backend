using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.ApiUtils
{
    public class RequestUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public List<string> Roles { get; set; }
    }
}
