using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.Framework.CacheMemory.Interfaces
{
    public interface ICacheBase
    {
        Task<T> GetOrCreate<T>(string key, Func<Task<T>> createEntry);
        void Remove(string key);
    }
}
