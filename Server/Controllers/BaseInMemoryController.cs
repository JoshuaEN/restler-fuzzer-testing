using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public abstract class BaseInMemoryController<TKey, TValue> : ControllerBase where TValue : IInMemoryModel<TKey>
    {
        private ConcurrentDictionary<TKey, TValue> store = new ConcurrentDictionary<TKey, TValue>();

        protected IList<TValue> _Index()
        {
            return this.store.Values.ToList();
        }

        protected ActionResult<TValue> _Create(TKey key, TValue value)
        {
            {
                var check = CheckPair(key, value);
                if (check != null)
                {
                    return check;
                }
            }

            if (this.store.TryAdd(key, value))
            {
                return Ok(value);
            }
            return Conflict();
        }

        protected ActionResult<TValue> _GetOrCreate(TKey key, TValue value)
        {
            {
                var check = CheckPair(key, value);
                if (check != null)
                {
                    return check;
                }
            }

            // Note: In the real implementation, this would be comparing the existing TValue and the new value to ensure they were completely equal.
            return Ok(this.store.GetOrAdd(key, value));
        }

        protected ActionResult<TValue> _CreateOrUpdate(TKey key, TValue value)
        {
            {
                var check = CheckPair(key, value);
                if (check != null)
                {
                    return check;
                }
            }

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
