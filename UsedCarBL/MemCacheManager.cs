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
        TimeSpan _RefreshCacheDuration = new TimeSpan(0, 1, 0);
        private static MemcachedClient mc = null;
        
        static MemCacheManager()
        {
            try
            {
                mc = new MemcachedClient("memcached");
                //MemcachedClientConfiguration config = new MemcachedClientConfiguration();
                //config.Servers.Add(new IPEndPoint(IPAddress.Parse("172.16.1.39"), 11211));
                //config.Protocol = MemcachedProtocol.Binary;
                //config.Authentication.Type = typeof();
                //mc = new MemcachedClient(config);
            }
            catch (Exception err)
            {
                Console.Write(err);
                throw;
            }
            
          
        }

        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback)
        {
            key = key + "_AS";
            T t = default(T);
            try
            {
                t = mc.Get<T>(key);
                if (EqualityComparer<T>.Default.Equals(t, default(T)))
                {
                    bool a;
                    if (a = mc.Store(StoreMode.Add, key + "_AS_lock", "lock", DateTime.Now.AddSeconds(60)))
                    {
                        t = dbCallback();

                        mc.Store(StoreMode.Add, key, t, DateTime.Now.Add(cacheDuration));

                        mc.Remove(key + "_AS_lock");
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
                Console.Write(err);
                throw;
            }
        }

        public bool DeleteFromCache(string key)
        {
            try
            {
                return mc.Remove(key);
            }
            catch (Exception err)
            {
                Console.Write(err);
                return false;
            }
        }


        //public void RefreshCache(string key)
        //{
        //    try
        //    {
        //        AddDelay(key);
        //    }
        //    catch (Exception err)
        //    {
        //        Console.Write(err);
        //    }
        //}




        //public bool Update(string key, object newValue, TimeSpan time)
        //{
        //    try
        //    {
        //        bool success = true;

        //        if (mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
        //        {
        //            success = mc.Store(StoreMode.Set, key, newValue, DateTime.Now.Add(time));
        //            mc.Remove(key + "_lock");
        //        }
        //        return success;
        //    }
        //    catch (Exception err)
        //    {
        //        Console.Write(err);
        //    }
        //    return false;
        //}


        //public T GetKey<T>(string key)
        //{

        //    T t = default(T);

        //    try
        //    {

        //        t = (T)mc.Get<T>(key);

        //    }
        //    catch (Exception err)
        //    {
        //        Console.Write(err);
        //    }
        //    return t;
        //}

        //private void AddDelay(string key)
        //{
        //    try
        //    {
        //        object obj = null;

        //        obj = GetKey<object>(key);
        //        if (obj != null)
        //        {
        //            Update(key, obj, _RefreshCacheDuration);
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        Console.Write(err);
        //    }
        //}

    }
}
