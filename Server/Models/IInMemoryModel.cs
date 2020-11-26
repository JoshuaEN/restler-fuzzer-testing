using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Models
{
    public interface IInMemoryModel<TKey>
    {
        [JsonIgnore]
        TKey InternalIdentifier { get; set; }
    }
}
