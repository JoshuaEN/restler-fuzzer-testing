using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Attributes;
using Server.Models;
using Server.Services;
using Server.Utilities;

namespace Server.Controllers
{
#if ENDPOINT_NESTED
    [Route("api/v1/AsExpects/{asExpectId}/NestedResourceAdjacents")]
    [ApiController]
    public class NestedResourceAdjacentController : BaseInMemoryController<NestedResourceAdjacentController, (string asExpectId, string nestedResourceAdjacentId), NestedResourceAdjacent>
    {
        private static Func<(string asExpectId, string nestedResourceAdjacentId), (string asExpectId, string nestedResourceAdjacentId)> GetUniqueNestedResourceAdjacentId = (id) => (id.asExpectId, UniqueValueFunctions.GetUniqueString());
        private readonly ConcurrentDictionary<string, AsExpect> asExpectedItems;
        private readonly ConcurrentDictionary<(string asExpectId, string nestedResourceId), NestedResource> nestedResourceItems;
        public NestedResourceAdjacentController(
            InMemoryStorageService<AsExpectController, string, AsExpect> asExpectStorageService,
            InMemoryStorageService<NestedResourceController, (string asExpectId, string nestedResourceId), NestedResource> nestedResourceStorageService,
            InMemoryStorageService<NestedResourceAdjacentController, (string asExpectId, string nestedResourceAdjacentId), NestedResourceAdjacent> storageService)
            : base(InMemoryStorageMode.MimicExpectedByRESTler, NestedResourceAdjacentController.GetUniqueNestedResourceAdjacentId, storageService)
        {
            asExpectedItems = asExpectStorageService.GetStorage();
            nestedResourceItems = nestedResourceStorageService.GetStorage();
        }

        [HttpGet]
        public ActionResult<IList<NestedResourceAdjacent>> Index([BindRequired] string asExpectId) {
            if (!asExpectedItems.ContainsKey(asExpectId))
            {
                return NotFound();
            }
            return Ok(base._Index());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asExpectId" exmaple="default"></param>
        /// <param name="nestedResourceAdjacentId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{nestedResourceAdjacentId}")]
        public ActionResult<NestedResourceAdjacent> Get([BindRequired] string asExpectId, [BindRequired] string nestedResourceAdjacentId) {
            var result = base._Get((asExpectId, nestedResourceAdjacentId));

            // Handle edge case where dependent resource is deleted, simulating a FK database relationship
            var check = result.Value == null ? null : CheckDependenciesExist(asExpectId, result.Value.NestedResourceId);
            if (check != null)
            {
                return check;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nestedResourceAdjacent"></param>
        /// <returns></returns>
        [HttpPost]
        [ResterAnnotation(producerEndpoint: "/api/v1/AsExpects", producerMethod: "POST", producerResourceName: "id", consumerParam: "asExpectId")]
        [ResterAnnotation(producerEndpoint: "/api/v1/AsExpects/{asExpectId}/NestedResources", producerMethod: "POST", producerResourceName: "id", consumerParam: "nestedResourceId")]
        public ActionResult<NestedResourceAdjacent> Create([BindRequired] string asExpectId, [FromBody, BindRequired] NestedResourceAdjacent nestedResourceAdjacent)
        {
            var check = CheckDependenciesExist(asExpectId, nestedResourceAdjacent.NestedResourceId) ?? CheckPair(asExpectId, nestedResourceAdjacent);
            if (check != null)
            {
                return check;
            }

            return base._Create(nestedResourceAdjacent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asExpectId example="default""></param>
        /// <param name="nestedResourceAdjacentId" example="default"></param>
        /// <param name="nestedResourceAdjacent"></param>
        /// <returns></returns>
        [HttpPut("{nestedResourceAdjacentId}")]
        [ResterAnnotation(producerEndpoint: "/api/v1/AsExpects", producerMethod: "POST", producerResourceName: "id", consumerParam: "asExpectId")]
        [ResterAnnotation(producerEndpoint: "/api/v1/AsExpects/{asExpectId}/NestedResources", producerMethod: "POST", producerResourceName: "id", consumerParam: "nestedResourceId")]
        public ActionResult<NestedResourceAdjacent> CreateOrUpdate([BindRequired] string asExpectId, [BindRequired] string nestedResourceAdjacentId, [FromBody, BindRequired] NestedResourceAdjacent nestedResourceAdjacent)
        {
            var check = CheckDependenciesExist(asExpectId, nestedResourceAdjacent.NestedResourceId) ?? CheckPair(asExpectId, nestedResourceAdjacent);
            if (check != null)
            {
                return check;
            }

            return base._CreateOrUpdate((asExpectId, nestedResourceAdjacentId), nestedResourceAdjacent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nestedResourceAdjacentId" example="default"></param>
        /// <returns></returns>
        [HttpDelete("{nestedResourceAdjacentId}")]
        public ActionResult Delete([BindRequired] string asExpectId, [BindRequired] string nestedResourceAdjacentId)
        {
            if (!asExpectedItems.ContainsKey(asExpectId))
            {
                return NotFound();
            }

            if (store.TryGetValue((asExpectId, nestedResourceAdjacentId), out NestedResourceAdjacent value))
            {
                var check = CheckDependenciesExist(asExpectId, value.NestedResourceId);
                if (check != null)
                {
                    return check;
                }
            }
            
            return base._Delete((asExpectId, nestedResourceAdjacentId));
        }

        private ActionResult<NestedResourceAdjacent> CheckPair(string asExpectId, NestedResourceAdjacent nestedResource)
        {
            if (nestedResource.AsExpectId != asExpectId)
            {
                return BadRequest(new { error = $"Expected AsExpectId {asExpectId} from URL to match asExpectId {nestedResource.AsExpectId} in body" });
            }

            return null;
        }

        private NotFoundResult CheckDependenciesExist(string asExpectId, string nestedResourceId)
        {
            if (!asExpectedItems.ContainsKey(asExpectId))
            {
                return NotFound();
            }
            if (!nestedResourceItems.ContainsKey((asExpectId, nestedResourceId)))
            {
                return NotFound();
            }
            return null;
        }
    }
#endif
}
