using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Enyim.Caching.Configuration;
using UsedCarEntities;
namespace UsedCarBL
{
    public class MemCacheManager
    {
        string KeyAppender = "_AS";
        TimeSpan _RefreshCacheDuration = new TimeSpan(0, 1, 0);
        private static MemcachedClient mc = null;
        static MemCacheManager()
        {
            try
            {
                mc = new MemcachedClient("memcached");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback)
        {
            key = key + KeyAppender;
            T t = default(T);
            try
            {
                t = mc.Get<T>(key);
                if (EqualityComparer<T>.Default.Equals(t, default(T)))
                {
                    bool a;
                    if (a = mc.Store(StoreMode.Add, key + KeyAppender + "lock", "lock", DateTime.Now.AddSeconds(60)))
                    {
                        t = dbCallback();
                        mc.Remove(key);
                        mc.Store(StoreMode.Add, key, t, DateTime.Now.Add(cacheDuration));
                        mc.Remove(key + KeyAppender + "lock");
                    }
                    else
                    {
                        t = dbCallback();
                    }
                    return t;
                }
                else
                    return t;
            }
            catch (Exception err)
            {
                throw;
            }
        }
        public bool DeleteFromCache(string key)
        {
            try
            {
                return mc.Remove(key);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
