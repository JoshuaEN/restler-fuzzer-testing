using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Traditional : IInMemoryModel<string>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
