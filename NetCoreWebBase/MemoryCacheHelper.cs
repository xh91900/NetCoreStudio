using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebBase
{
    public class MemoryCacheHelper
    {
        private static IMemoryCache _memoryCache = null;

        static MemoryCacheHelper()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public static void SetMemory(string key, object value)
        {
            _memoryCache.Set(key, value, TimeSpan.FromSeconds(60));
        }

        public static object GetMemory(string key)
        {
            if (_memoryCache.TryGetValue(key, out object value))
            {
                return value;
            }

            return null;
        }
    }
}
