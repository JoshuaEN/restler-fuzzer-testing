using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Utilities
{
    public static class UniqueValueFunctions
    {
        public static readonly Func<string> GetUniqueString = () => Guid.NewGuid().ToString();
    }
}
