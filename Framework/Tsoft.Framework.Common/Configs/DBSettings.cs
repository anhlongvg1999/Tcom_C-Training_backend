using System;
using System.Collections.Generic;
using System.Text;

namespace Tsoft.Framework.Common.Configs
{
    public class DBSettings
    {
        public string Prefix { get; set; }
        public string ConnectionString { get; set; }
        public string IsTesting { get; set; }
        public string DbType { get; set; }
    }
}
