using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    /// <summary>
    /// Determines how the in-memory storage controller behaves.
    /// </summary>
    public enum InMemoryStorageMode {
        /// <summary>
        /// In this mode, the storage controller will:
        /// 1. Assign a new unique ID when creating a new resource via POST /resources
        /// 1. Assign the body's resource Id to the URL's resource Id when creating or modifying a resource via POST|PUT /resources/{resourceId}
        /// </summary>
        MimicPrivateAPI,
        MimicExpectedByRESTler
    }

    public abstract class BaseInMemoryController<TController, TKey, TValue> : ControllerBase where TController : BaseInMemoryController<TController, TKey, TValue> where TValue : IInMemoryModel<TKey>
    {
        private readonly ConcurrentDictionary<TKey, TValue> store = new ConcurrentDictionary<TKey, TValue>();
        private readonly InMemoryStorageMode mode;
        private readonly Func<TKey> getUniqueKeyValue;

        protected BaseInMemoryController(InMemoryStorageMode mode, Func<TKey> getUniqueKeyValue, InMemoryStorageService<TController, TKey, TValue> storageService)
        {
            if (storageService == null)
            {
                throw new ArgumentNullException(nameof(storageService));
            }

            store = storageService.GetStorage();
            this.mode = mode;
            this.getUniqueKeyValue = getUniqueKeyValue;
        }

        protected IList<TValue> _Index()
        {
            return this.store.Values.ToList();
        }

        protected ActionResult<TValue> _Create(TValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.Id == null && mode == InMemoryStorageMode.MimicPrivateAPI)
            {
                throw new ArgumentNullException(nameof(value.Id));
            }

            if (mode == InMemoryStorageMode.MimicExpectedByRESTler)
            {
                value.Id = this.getUniqueKeyValue();
            }


            if (this.store.TryAdd(value.Id, value))
            {
                return Ok(value);
            }
            if (mode == InMemoryStorageMode.MimicPrivateAPI)
            {
                return Conflict();
            }

            if (this.store.TryGetValue(value.Id, out TValue existingValue))
            {
                return Ok(existingValue);
            }
            throw new Exception($"Failed to create resource with ID {value.Id} because resource appeared to exist but then could not be found");
        }

        protected ActionResult<TValue> _GetOrCreate(TKey key, TValue value)
        {
            if (mode == InMemoryStorageMode.MimicPrivateAPI)
            {
                var check = CheckPair(key, value);
                if (check != null)
                {
                    return check;
                }
            }

            value.Id = key;

            // Note: In the real implementation, this would be comparing the existing TValue and the new value to ensure they were completely equal.
            return Ok(this.store.GetOrAdd(key, value));
        }

        protected ActionResult<TValue> _CreateOrUpdate(TKey key, TValue value)
        {
            if (mode == InMemoryStorageMode.MimicPrivateAPI)
            {
                var check = CheckPair(key, value);
                if (check != null)
                {
                    return check;
                }
            }

            value.Id = key;

            return Ok(this.store.AddOrUpdate(key, value, (key, oldValue) => value));
        }

        protected ActionResult<TValue> _Get(TKey key)
        {
            if (this.store.TryGetValue(key, out TValue value))
            {
                return value;
            }
            return NotFound();
        }

        protected ActionResult _Delete(TKey key)
        {
            if (this.store.TryRemove(key, out _))
            {
                return NoContent();
            }
            return NotFound();
        }

        private ActionResult<TValue> CheckPair(TKey key, TValue value)
        {
            if (EqualityComparer<TKey>.Default.Equals(key, value.Id) != true)
            {
                return BadRequest(new { error = $"Expected Id {key} from URL to match Id {value.Id} in body" });
            }
            return null;
        }
    }
}
