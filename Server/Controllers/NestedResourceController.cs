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
    [Route("api/v1/AsExpects/{asExpectId}/NestedResources")]
    [ApiController]
    public class NestedResourceController : BaseInMemoryController<NestedResourceController, (string asExpectId, string nestedResourceId), NestedResource>
    {
        private static Func<(string asExpectId, string nestedResourceId), (string asExpectId, string nestedResourceId)> GetUniqueNestedResourceId = (id) => (id.asExpectId, UniqueValueFunctions.GetUniqueString());
        private readonly ConcurrentDictionary<string, AsExpect> asExpectedItems;
        public NestedResourceController(InMemoryStorageService<AsExpectController, string, AsExpect> asExpectStorageService, InMemoryStorageService<NestedResourceController, (string asExpectId, string nestedResourceId), NestedResource> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, NestedResourceController.GetUniqueNestedResourceId, storageService)
        {
            asExpectedItems = asExpectStorageService.GetStorage();
        }

        [HttpGet]
        public ActionResult<IList<NestedResource>> Index([BindRequired] string asExpectId) {
            var check = CheckAsExpectExists(asExpectId);
            if (check != null)
            {
                return check;
            }
            return Ok(base._Index());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asExpectId" exmaple="default"></param>
        /// <param name="nestedResourceId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{nestedResourceId}")]
        public ActionResult<NestedResource> Get([BindRequired] string asExpectId, [BindRequired] string nestedResourceId)
        {
            var check = CheckAsExpectExists(asExpectId);
            if (check != null)
            {
                return check;
            }

            return base._Get((asExpectId, nestedResourceId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nestedResource"></param>
        /// <returns></returns>
        [HttpPost]
        [ResterAnnotation(producerEndpoint: "/api/v1/AsExpects", producerMethod: "POST", producerResourceName: "id", consumerParam: "asExpectId")]
        public ActionResult<NestedResource> Create([BindRequired] string asExpectId, [FromBody, BindRequired] NestedResource nestedResource)
        {
            var check =  CheckAsExpectExists(asExpectId) ?? CheckPair(asExpectId, nestedResource);
            if (check != null)
            {
                return check;
            }

            return base._Create(nestedResource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asExpectId example="default""></param>
        /// <param name="nestedResourceId" example="default"></param>
        /// <param name="nestedResource"></param>
        /// <returns></returns>
        [HttpPut("{nestedResourceId}")]
        [ResterAnnotation(producerEndpoint: "/api/v1/AsExpects", producerMethod: "POST", producerResourceName: "id", consumerParam: "asExpectId")]
        public ActionResult<NestedResource> CreateOrUpdate([BindRequired] string asExpectId, [BindRequired] string nestedResourceId, [FromBody, BindRequired] NestedResource nestedResource)
        {
            var check = CheckAsExpectExists(asExpectId) ?? CheckPair(asExpectId, nestedResource);
            if (check != null)
            {
                return check;
            }

            return base._CreateOrUpdate((asExpectId, nestedResourceId), nestedResource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nestedResourceId" example="default"></param>
        /// <returns></returns>
        [HttpDelete("{nestedResourceId}")]
        public ActionResult Delete([BindRequired] string asExpectId, [BindRequired] string nestedResourceId)
        {
            var check = CheckAsExpectExists(asExpectId);
            if (check != null)
            {
                return check;
            }

            return base._Delete((asExpectId, nestedResourceId));
        }

        private ActionResult<NestedResource> CheckPair(string asExpectId, NestedResource nestedResource)
        {
            if (nestedResource.AsExpectId != asExpectId)
            {
                return BadRequest(new { error = $"Expected AsExpectId {asExpectId} from URL to match asExpectId {nestedResource.AsExpectId} in body" });
            }

            return null;
        }

        private NotFoundResult CheckAsExpectExists(string asExpectId)
        {
            if (!asExpectedItems.ContainsKey(asExpectId))
            {
                return NotFound();
            }
            return null;
        }
    }
#endif
}
