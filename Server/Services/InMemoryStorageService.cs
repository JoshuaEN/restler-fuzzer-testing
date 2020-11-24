using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Server.Services
{
    public class InMemoryStorageService<TController, TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> storage = new ConcurrentDictionary<TKey, TValue>();
        public ConcurrentDictionary<TKey, TValue> GetStorage()
        {
            return this.storage;
        }
    }
}
