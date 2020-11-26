using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class NestedResourceAdjacent : IInMemoryModel<(string asExpectId, string nestedResourceAdjacentId)>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string AsExpectId { get; set; }

        [Required]
        public string NestedResourceId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public (string asExpectId, string nestedResourceAdjacentId) InternalIdentifier { get => (AsExpectId, Id); set => (AsExpectId, Id) = value; }
    }
}
