using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.CrossCuttingConcern.Caching
{
    public static class CacheExtension
    {
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            //eğer değer daha önce cache edilmiş ise değeri getir 
            if (cacheManager.IsAdd(key))
                return cacheManager.Get<T>(key);

            //değer cache de yok ise ekle
            var result = acquire();
            cacheManager.Add(key, result, cacheTime);
            return result;
        }
    }
}
