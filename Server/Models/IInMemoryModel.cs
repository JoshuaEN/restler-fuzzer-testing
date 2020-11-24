using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public interface IInMemoryModel<TKey>
    {
        TKey Id { get; set; }
    }
}
