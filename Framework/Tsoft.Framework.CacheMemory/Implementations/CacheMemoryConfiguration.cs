using System;
using System.Collections.Generic;
using System.Text;
using TSoft.Framework.CacheMemory.Interfaces;

namespace TSoft.Framework.CacheMemory.Implementations
{
    public class CacheMemoryConfiguration : ICacheMemoryConfiguration
    {
        public bool EnableCache { get ; set ; }
    }
}
