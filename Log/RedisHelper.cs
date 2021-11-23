using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    internal class RedisHelper
    {
        private static readonly object ObjectLock = new();

        private static volatile IDatabase _db;
        public static IDatabase Db
        {
            get
            {
                if (_db == null)
                {
                    lock (ObjectLock)
                    {
                        if (_db == null)
                        {
                            var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                            _db = redis.GetDatabase();
                        }
                    }
                }

                return _db;
            }
        }
    }
}
